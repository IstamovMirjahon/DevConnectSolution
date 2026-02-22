namespace DevConnect.Application.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
public class SuccessResponseAttribute(Type type, string message) : Attribute
{
    public Type Type { get; set; } = type;
    public string? Message { get; set; } = message;
}