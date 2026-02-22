namespace DevConnect.Domain.Helpers;

public class ValidationError(List<FieldError> errors, string message = "Validation failed") : UserError(CommonErrorCodes.BadInput, message)
{
    public List<FieldError> Errors { get; set; } = errors;
}