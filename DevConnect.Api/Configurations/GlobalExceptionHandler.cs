using DevConnect.Domain.Helpers;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;

namespace DevConnect.Api.Configurations;

public sealed class GlobalExceptionHandler
     (ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
       HttpContext httpContext,
       Exception exception,
       CancellationToken cancellationToken)
    {
        logger.LogError(
            exception,
            "Exception occurred in OwnerOperations service: {Message}",
            exception.Message
        );

        var response = new InternalServerError();

        httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);

        return true;
    }
}
