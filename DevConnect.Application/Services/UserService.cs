using DevConnect.Application.Interfaces;
using DevConnect.Application.Models.Users;
using DevConnect.Domain.Enums;
using DevConnect.Domain.Helpers;
using DevConnect.Domain.IRepositories;
using Microsoft.AspNetCore.Http;

namespace DevConnect.Application.Services;

public class UserService(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher,
    IUnitOfWork unitOfWork,
    IFileStorageService fileStorageService) : IUserService
{
    public async Task<Result<UserProfileResponse>> GetCurrentProfileAsync(Guid userId, CancellationToken ct = default)
    {
        var user = await userRepository.GetByIdAsync(userId, ct);
        if (user is null)
        {
            return Result<UserProfileResponse>.Fail(new NotFoundError(ErrorCodes.UserNotFound, ErrorMessages.UserNotFound));
        }

        var response = new UserProfileResponse
        {
            Id = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            ImageUrl = user.ImageUrl,
            PortfolioUrl = user.PortfolioUrl,
            Role = user.Role,
            Profession = user.Profession,
            Type = user.Type,
            State = user.State
        };

        return Result<UserProfileResponse>.Success(response);
    }

    public async Task<Result> ChangePasswordAsync(Guid userId, UpdatePasswordRequest request, CancellationToken ct = default)
    {
        var user = await userRepository.GetByIdAsync(userId, ct);
        if (user is null)
        {
            return Result.Fail(new NotFoundError(ErrorCodes.UserNotFound, ErrorMessages.UserNotFound));
        }

        if (!passwordHasher.Verify(request.CurrentPassword, user.PasswordHash))
        {
            return Result.Fail(new UserError(ErrorCodes.PasswordMismatch, ErrorMessages.IncorrectCurrentPassword));
        }

        var newHash = passwordHasher.Hash(request.NewPassword);
        user.ChangePassword(newHash);

        userRepository.Update(user);
        await unitOfWork.SaveChangesAsync(ct);

        return Result.Success();
    }

    public async Task<Result> UpdateUserTypeAsync(Guid targetUserId, UserType newType, CancellationToken ct = default)
    {
        var user = await userRepository.GetByIdAsync(targetUserId, ct);
        if (user is null)
            return Result.Fail(new NotFoundError(ErrorCodes.UserNotFound, ErrorMessages.UserNotFound));

        if (user.Type == newType)
            return Result.Fail(new UserError(ErrorCodes.InvalidUserType, ErrorMessages.UserAlreadyHasType));

        user.SetUserType(newType);
        userRepository.Update(user);
        await unitOfWork.SaveChangesAsync(ct);

        return Result.Success();
    }

    public async Task<Result<UploadAvatarResponse>> UploadAvatarAsync(Guid userId, IFormFile file, CancellationToken ct = default)
    {
        var user = await userRepository.GetByIdAsync(userId, ct);
        if (user is null)
            return Result<UploadAvatarResponse>.Fail(new NotFoundError(ErrorCodes.UserNotFound, ErrorMessages.UserNotFound));

        var imageUrl = await fileStorageService.SaveFileAsync(file, ct);

        await userRepository.UpdateImageUrlAsync(userId, imageUrl, ct);

        return Result<UploadAvatarResponse>.Success(new UploadAvatarResponse { ImageUrl = imageUrl });
    }
}
