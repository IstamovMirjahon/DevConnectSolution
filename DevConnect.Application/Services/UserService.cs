using DevConnect.Application.Interfaces;
using DevConnect.Application.Models.Users;
using DevConnect.Domain.Helpers;
using DevConnect.Domain.IRepositories;

namespace DevConnect.Application.Services;

public class UserService(IUserRepository userRepository, IPasswordHasher passwordHasher, IUnitOfWork unitOfWork) : IUserService
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
}
