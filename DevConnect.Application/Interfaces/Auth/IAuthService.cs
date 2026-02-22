using DevConnect.Application.Models.Auth.Requests;
using DevConnect.Application.Models.Auth.Response;
using DevConnect.Domain.Helpers;

namespace DevConnect.Application.Interfaces.Auth;

public interface IAuthService
{
    Task<Result<AuthResponse>> RegisterAsync(
        RegisterRequest request,
        CancellationToken ct);

    Task<Result<AuthResponse>> LoginAsync(
        LoginRequest request,
        CancellationToken ct);
}