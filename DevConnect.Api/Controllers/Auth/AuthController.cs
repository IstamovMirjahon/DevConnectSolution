using DevConnect.Application.Interfaces.Auth;
using DevConnect.Application.ResponseSerializer;
using DevConnect.Application.Models.Auth.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DevConnect.Domain.Helpers;

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

    [HttpPost("resend-code")]
    public async Task<IActionResult> ResendCode([FromBody] ResendCodeRequest request, CancellationToken ct)
    {
        var result = await authService.ResendVerificationCodeAsync(request.Email, ct);

        if (result.IsSuccess)
            return serializer.ToActionResult(Result.Success("Verification code successfully resent"));

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
        var result = await authService.RefreshAsync(refreshToken, ct);

        if (result.IsSuccess)
            return serializer.ToActionResult(Result.Success(new { accessToken = result.Value }));

        return serializer.ToActionResult(result);
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
        var result = await authService.RequestPasswordResetAsync(email, ct);

        if (result.IsSuccess)
            return serializer.ToActionResult(Result.Success("Reset code sent to email"));

        return serializer.ToActionResult(result);
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword(ResetPasswordRequest request, CancellationToken ct)
    {
        var result = await authService.ResetPasswordAsync(
            request.Email,
            request.Code,
            request.NewPassword,
            ct);

        if (result.IsSuccess)
            return serializer.ToActionResult(Result.Success("Password successfully reset"));

        return serializer.ToActionResult(result);
    }
}