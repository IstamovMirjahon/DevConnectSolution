using System.Security.Claims;
using DevConnect.Application.Interfaces;
using DevConnect.Application.Models.Users;
using DevConnect.Application.ResponseSerializer;
using DevConnect.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevConnect.Api.Controllers.Users;

[ApiController]
[Route("api/users")]
[Authorize]
public class UserController(
    IUserService userService, 
    IUserNotificationService notificationService,
    DevConnectResponseSerializer serializer) : ControllerBase
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

    [HttpGet("me/notifications")]
    public async Task<IActionResult> GetMyNotifications(CancellationToken ct)
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
        if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
            return Unauthorized(new { message = "User is not authorized." });

        var result = await notificationService.GetUserNotificationsAsync(userId, ct);
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

    [HttpPatch("{id:guid}/type")]
    //[Authorize(Roles = "LogCheckerCompany,LogCheckerDeveloper,Admin,SystemAdmin")]
    public async Task<IActionResult> UpdateUserType(
        [FromRoute] Guid id,
        [FromQuery] UserType type,
        CancellationToken ct)
    {
        var result = await userService.UpdateUserTypeAsync(id, type, ct);
        return serializer.ToActionResult(result);
    }

    [HttpPost("me/avatar")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UploadAvatar(IFormFile file, CancellationToken ct)
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
        if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
            return Unauthorized(new { message = "User is not authorized." });

        if (file is null || file.Length == 0)
            return BadRequest(new { message = "File is empty." });

        var result = await userService.UploadAvatarAsync(userId, file, ct);
        return serializer.ToActionResult(result);
    }

    [HttpPost("me/cv")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UploadCv(IFormFile file, CancellationToken ct)
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
        if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
            return Unauthorized(new { message = "User is not authorized." });

        if (file is null || file.Length == 0)
            return BadRequest(new { message = "File is empty." });

        var result = await userService.UploadCvAsync(userId, file, ct);
        return serializer.ToActionResult(result);
    }
}