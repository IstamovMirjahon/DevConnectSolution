using System.Security.Claims;
using DevConnect.Application.Interfaces;
using DevConnect.Application.Models.Recruiters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DevConnect.Application.ResponseSerializer;

namespace DevConnect.Api.Controllers.Recruiters;

[ApiController]
[Route("api/recruiter")]
public class RecruiterController
        (IRecruiterService recruiterService, 
        DevConnectResponseSerializer serializer) : ControllerBase
{
    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] CreateRecruiterRequest request, CancellationToken ct)
    {   
        var result = await recruiterService.CreateRecruiterAsync(request, ct);
        return serializer.ToActionResult(result);
    }
}