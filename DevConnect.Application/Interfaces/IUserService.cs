using DevConnect.Application.Models.Users;
using DevConnect.Domain.Enums;
using DevConnect.Domain.Helpers;
using Microsoft.AspNetCore.Http;

namespace DevConnect.Application.Interfaces;

public interface IUserService
{
    Task<Result<UserProfileResponse>> GetCurrentProfileAsync(Guid userId, CancellationToken ct = default);
    Task<Result> ChangePasswordAsync(Guid userId, UpdatePasswordRequest request, CancellationToken ct = default);
    Task<Result> UpdateUserTypeAsync(Guid targetUserId, UserType newType, CancellationToken ct = default);
    Task<Result<UploadAvatarResponse>> UploadAvatarAsync(Guid userId, IFormFile file, CancellationToken ct = default);
    Task<Result<UploadCvResponse>> UploadCvAsync(Guid userId, IFormFile file, CancellationToken ct = default);
}