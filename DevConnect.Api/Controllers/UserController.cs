using System.Security.Claims;
using DevConnect.Application.Interfaces;
using DevConnect.Application.Models.Users;
using DevConnect.Application.ResponseSerializer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevConnect.Api.Controllers.Users;

[ApiController]
[Route("api/users")]
public class UserController(IUserService userService, DevConnectResponseSerializer serializer) : ControllerBase
{
    [HttpGet("me")]
    public async Task<IActionResult> GetMyProfile(CancellationToken ct)
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
        if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
            return Unauthorized(new { message = "User is not authorized." });

        var result = await userService.GetCurrentProfileAsync(userId, ct);
        return serializer.ToActionResult(result);
    }
    [HttpPut("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] UpdatePasswordRequest request, CancellationToken ct)
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
        if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
            return Unauthorized(new { message = "User is not authorized." });

        var result = await userService.ChangePasswordAsync(userId, request, ct);
        return serializer.ToActionResult(result);
    }
}