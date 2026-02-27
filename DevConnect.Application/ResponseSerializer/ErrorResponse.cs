using DevConnect.Domain.Helpers;

namespace DevConnect.Application.ResponseSerializer;

public class ErrorResponse
{
    public ErrorResponse(string code, string? message)
    {
        Code = code;
        Message = message;
    }

    public string Code { get; set; }
    public string? Message { get; set; }
    public List<FieldError>? Errors { get; set; }
}
