using DevConnect.Application.Interfaces;
using DevConnect.Application.ResponseSerializer;
using DevConnect.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevConnect.Api.Controllers.LogChecker;

[ApiController]
[Route("api/logchecker-company")]
//[Authorize(Roles = "LogCheckerCompany")]
public class LogCheckerCompanyController(
    ILogCheckerCompanyService logCheckerCompanyService,
    DevConnectResponseSerializer serializer) : ControllerBase
{
    [HttpGet("recruiters")]
    public async Task<IActionResult> GetRecruiters(
        [FromQuery] UserType? type,
        CancellationToken ct)
    {
        var result = await logCheckerCompanyService.GetRecruitersAsync(type, ct);
        return serializer.ToActionResult(result);
    }
}
