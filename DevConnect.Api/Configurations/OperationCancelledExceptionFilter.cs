using Microsoft.AspNetCore.Mvc.Filters;

namespace DevConnect.Api.Configurations;

public class OperationCancelledExceptionFilter(ILogger<OperationCancelledExceptionFilter> logger) : IAsyncExceptionFilter
{
    public Task OnExceptionAsync(ExceptionContext context)
    {
        if (context.Exception is OperationCanceledException &&
            context.HttpContext.RequestAborted.IsCancellationRequested)
        {
            // mark handled so it doesn’t become a 500
            context.ExceptionHandled = true;

            context.HttpContext.Response.Clear();
            // optional: 499 is a non-standard “Client Closed Request”
            context.HttpContext.Response.StatusCode = 499;

            logger.LogDebug("Request aborted by client, swallowed cancellation.");
        }

        return Task.CompletedTask;
    }
}