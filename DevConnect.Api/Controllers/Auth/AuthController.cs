using DevConnect.Application.Interfaces.Auth;
using DevConnect.Application.ResponseSerializer;
using DevConnect.Application.Models.Auth.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace DevConnect.Api.Controllers.Auth;

[ApiController]
[Route("api/auth")]
[AllowAnonymous]
public class AuthController(IAuthService authService, DevConnectResponseSerializer serializer) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register(Application.Models.Auth.Requests.RegisterRequest request, CancellationToken ct)
    {
        var result = await authService.RegisterAsync(request, ct);
        return serializer.ToActionResult(result);
    }
    [HttpPost("verify-register")]
    public async Task<IActionResult> VerifyRegister(string email, string code, CancellationToken ct)
    {
        var result = await authService
            .VerifyRegisterAsync(email, code, ct);

        return serializer.ToActionResult(result);
    }
    [HttpPost("login")]
    public async Task<IActionResult> Login(Application.Models.Auth.Requests.LoginRequest request, CancellationToken ct)
    {
        var result = await authService.LoginAsync(request, ct);
        return serializer.ToActionResult(result);
    }
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh(string refreshToken, CancellationToken ct)
    {
        var newAccessToken =
            await authService.RefreshAsync(refreshToken, ct);

        return serializer.ToActionResult(new { accessToken = newAccessToken });
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout(string refreshToken, CancellationToken ct)
    {
        await authService.LogoutAsync(refreshToken, ct);

        return serializer.ToActionResult(null);
    }
    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword(string email, CancellationToken ct)
    {
        await authService
            .RequestPasswordResetAsync(email, ct);

        return serializer.ToActionResult("Reset code sent to email");
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword(Application.Models.Auth.Requests.ResetPasswordRequest request, CancellationToken ct)
    {
        await authService.ResetPasswordAsync(
            request.Email,
            request.Code,
            request.NewPassword,
            ct);

        return serializer.ToActionResult("Password successfully reset");
    }
}