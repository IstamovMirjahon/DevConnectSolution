using DevConnect.Application.Interfaces;
using DevConnect.Application.ResponseSerializer;
using DevConnect.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevConnect.Api.Controllers.LogChecker;

[ApiController]
[Route("api/logchecker-developer")]
//[Authorize(Roles = "LogCheckerDeveloper")]
public class LogCheckerDeveloperController(
    ILogCheckerDeveloperService logCheckerDeveloperService,
    DevConnectResponseSerializer serializer) : ControllerBase
{
    [HttpGet("developers")]
    public async Task<IActionResult> GetDevelopers(
        [FromQuery] UserType? type,
        CancellationToken ct)
    {
        var result = await logCheckerDeveloperService.GetDevelopersAsync(type, ct);
        return serializer.ToActionResult(result);
    }
}
