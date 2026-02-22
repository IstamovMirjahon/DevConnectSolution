using DevConnect.Application.Interfaces.Auth;
using DevConnect.Application.Models.Auth.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevConnect.Api.Controllers.Auth;

[ApiController]
[Route("api/auth")]
[AllowAnonymous]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request, CancellationToken ct)
    {
        var result = await authService.RegisterAsync(request, ct);

        if (result.IsFailed)
            return BadRequest(result.Error);

        return Ok(new { data = result.Value });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request, CancellationToken ct)
    {
        var result = await authService.LoginAsync(request, ct);

        if (result.IsFailed)
            return BadRequest(result.Error);

        return Ok(new { data = result.Value });
    }
}