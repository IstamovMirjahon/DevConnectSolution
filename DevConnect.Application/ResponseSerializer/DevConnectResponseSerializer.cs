using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;
using DevConnect.Domain.Helpers;

namespace DevConnect.Application.ResponseSerializer;

public class DevConnectResponseSerializer(ILogger<DevConnectResponseSerializer> logger)
{
    public ObjectResult ToActionResult(object? value)
    {
        return StatusCode((int)HttpStatusCode.OK, new
        {
            SuccessResult = value
        });
    }

    public ObjectResult ToActionResult(Result result)
    {
        if (result.IsSuccess)
            return StatusCode((int)HttpStatusCode.OK, null);

        return SerializeErrorResult(result);
    }

    public ObjectResult ToActionResult<T>(Result<T> result)
    {
        if (result.IsSuccess)
            return SerializeSuccessResult(result);

        return SerializeErrorResult(result);
    }


    private ObjectResult SerializeSuccessResult<T>(Result<T> result)
    {
        return StatusCode((int)HttpStatusCode.OK, new
        {
            SuccessResult = result.Value,
        });
    }

    private ObjectResult StatusCode(int statusCode, object? value)
    {
        return new ObjectResult(value)
        {
            StatusCode = statusCode
        };
    }

    private ObjectResult InternalServerError()
    {
        return StatusCode((int)HttpStatusCode.InternalServerError, new ErrorResponse(CommonErrorCodes.ServiceError, "Internal server error"));
    }

    private ObjectResult SerializeErrorResult(Result result)
    {
        return SerializeError(result.Error);
    }

    private ObjectResult SerializeError(Error? error)
    {
        switch (error)
        {
            case ValidationError validationError:
                return StatusCode((int)HttpStatusCode.UnprocessableContent,
                    new ErrorResponse(validationError.Code, validationError.Message)
                    {
                        Errors = validationError.Errors
                    });

            case NotFoundError:
                return StatusCode((int)HttpStatusCode.NotFound,
                    new ErrorResponse(error.Code, error.Message));

            case UserError:
                return StatusCode((int)HttpStatusCode.BadRequest,
                    new ErrorResponse(error.Code, error.Message));

            case InfrastructureError:
                return StatusCode((int)HttpStatusCode.InternalServerError,
                    new ErrorResponse(error.Code, error.Message));

            case ServiceError:
                return InternalServerError();

            default:
                logger.LogError("Unexpected Error result {Error}",
                    JsonSerializer.Serialize(error));
                return InternalServerError();
        }
    }
}