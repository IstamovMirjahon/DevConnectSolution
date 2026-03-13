using DevConnect.Application.Interfaces;
using DevConnect.Application.Models.Users;
using DevConnect.Domain.Enums;
using DevConnect.Domain.Helpers;
using DevConnect.Domain.IRepositories;
using Microsoft.AspNetCore.Http;
using UglyToad.PdfPig;
using System.Text;

namespace DevConnect.Application.Services;

public class UserService(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher,
    IUnitOfWork unitOfWork,
    IFileStorageService fileStorageService,
    ICvAnalyzerService cvAnalyzerService) : IUserService
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

    public async Task<Result<UploadCvResponse>> UploadCvAsync(Guid userId, IFormFile file, CancellationToken ct = default)
    {
        // 1. Faqat PDF qabul qilamiz
        if (!file.FileName.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase)
            && file.ContentType != "application/pdf")
        {
            return Result<UploadCvResponse>.Fail(
                new UserError(ErrorCodes.InvalidCvFile, ErrorMessages.InvalidCvFile));
        }

        // 2. Foydalanuvchini tekshiramiz
        var user = await userRepository.GetByIdAsync(userId, ct);
        if (user is null)
            return Result<UploadCvResponse>.Fail(
                new NotFoundError(ErrorCodes.UserNotFound, ErrorMessages.UserNotFound));

        // 3. CV faylni saqlaymiz
        var cvUrl = await fileStorageService.SaveFileAsync(file, ct);

        // 4. PDF dan matn o'qiymiz (PdfPig)
        string cvText;
        using (var stream = file.OpenReadStream())
        {
            using var pdfDoc = PdfDocument.Open(stream);
            var sb = new StringBuilder();
            foreach (var page in pdfDoc.GetPages())
                sb.AppendLine(page.Text);
            cvText = sb.ToString();
        }

        // 5. OpenAI orqali tahlil qilamiz
        var analysis = await cvAnalyzerService.AnalyzeCvAsync(cvText, ct);
        if (analysis is null)
        {
            return Result<UploadCvResponse>.Fail(
                new InfrastructureError(ErrorCodes.AiAnalysisFailed, ErrorMessages.AiAnalysisFailed));
        }

        // 6. User jadvalini yangilaymiz
        await userRepository.UpdateCvProfileAsync(
            userId,
            cvUrl,
            analysis.Title,
            analysis.Description,
            analysis.Skills,
            analysis.ExperienceLevel,
            analysis.PortfolioUrl,
            ct);

        // 7. Response qaytaramiz
        return Result<UploadCvResponse>.Success(new UploadCvResponse
        {
            CvUrl             = cvUrl,
            Title             = analysis.Title,
            Description       = analysis.Description,
            Skills            = analysis.Skills,
            ExperienceLevel   = analysis.ExperienceLevel,
            SuitableVacancies = analysis.SuitableVacancies,
            PortfolioUrl      = analysis.PortfolioUrl
        });
    }
}
