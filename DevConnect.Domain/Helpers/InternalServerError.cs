namespace DevConnect.Domain.Helpers;

public class InternalServerError(string message = "Internal Server Error") : ServiceError(CommonErrorCodes.ServiceError, message)
{
}