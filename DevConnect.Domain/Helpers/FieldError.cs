namespace DevConnect.Domain.Helpers;

public class FieldError(List<string> path, string code, string message)
{
    public List<string> Path { get; set; } = path;
    public string Code { get; set; } = code;
    public string Message { get; set; } = message;
}