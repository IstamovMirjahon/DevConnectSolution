using DevConnect.Application.Models.Auth.Requests;
using DevConnect.Application.Models.Auth.Response;
using DevConnect.Domain.Helpers;

namespace DevConnect.Application.Interfaces.Auth;

public interface IAuthService
{
    Task<Result<AuthResponse>> RegisterAsync(RegisterRequest request,CancellationToken ct);
    Task<Result<LoginResponse>> LoginAsync( LoginRequest request,CancellationToken ct);
    Task<Result<string>> RefreshAsync(string refreshToken, CancellationToken ct);
    Task<Result> LogoutAsync(string refreshToken, CancellationToken ct);
    Task<Result> RequestPasswordResetAsync(string email, CancellationToken ct);
    Task<Result> ResetPasswordAsync(string email, string code, string newPassword, CancellationToken ct);
    Task<Result> VerifyRegisterAsync(string email, string code, CancellationToken ct);
    Task<Result> ResendVerificationCodeAsync(string email, CancellationToken ct);
}