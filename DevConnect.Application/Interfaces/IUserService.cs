using DevConnect.Application.Models.Users;
using DevConnect.Domain.Helpers;

namespace DevConnect.Application.Interfaces;

public interface IUserService
{
    Task<Result<UserProfileResponse>> GetCurrentProfileAsync(Guid userId, CancellationToken ct = default);
    Task<Result> ChangePasswordAsync(Guid userId, UpdatePasswordRequest request, CancellationToken ct = default);
}
