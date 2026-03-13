using DevConnect.Application.Interfaces;
using DevConnect.Application.Models.Interviews;
using DevConnect.Application.ResponseSerializer;
using DevConnect.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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

    [HttpPost("schedule")]
    public async Task<IActionResult> ScheduleInterview([FromBody] ScheduleInterviewRequest request, CancellationToken ct)
    {
        var logCheckerId = GetUserId();
        if (logCheckerId == Guid.Empty)
            return Unauthorized(new { message = "User is not authorized." });

        var result = await logCheckerCompanyService.ScheduleInterviewAsync(logCheckerId, request, ct);
        return serializer.ToActionResult(result);
    }

    [HttpGet("interviews")]
    public async Task<IActionResult> GetInterviews([FromQuery] InterviewFilterRequest request, CancellationToken ct)
    {
        var logCheckerId = GetUserId();
        if (logCheckerId == Guid.Empty)
            return Unauthorized(new { message = "User is not authorized." });

        var result = await logCheckerCompanyService.GetInterviewsAsync(logCheckerId, request, ct);
        return serializer.ToActionResult(result);
    }

    [HttpPost("interviews/{interviewId:guid}/finish")]
    public async Task<IActionResult> FinishInterview([FromRoute] Guid interviewId, [FromBody] FinishRecruiterInterviewRequest request, CancellationToken ct)
    {
        var logCheckerId = GetUserId();
        if (logCheckerId == Guid.Empty)
            return Unauthorized(new { message = "User is not authorized." });

        var result = await logCheckerCompanyService.FinishInterviewAsync(logCheckerId, interviewId, request, ct);
        return serializer.ToActionResult(result);
    }

    [HttpGet("report")]
    public async Task<IActionResult> GetReport([FromQuery] TimePeriod timePeriod, CancellationToken ct)
    {
        var logCheckerId = GetUserId();
        if (logCheckerId == Guid.Empty)
            return Unauthorized(new { message = "User is not authorized." });

        var result = await logCheckerCompanyService.GetReportAsync(logCheckerId, timePeriod, ct);
        return serializer.ToActionResult(result);
    }

    private Guid GetUserId()
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
        if (!string.IsNullOrEmpty(userIdString) && Guid.TryParse(userIdString, out var userId))
        {
            return userId;
        }
        return Guid.Empty;
    }
}
