namespace DevConnect.Application.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
public class ProducesApiErrorAttribute(string error, string description) : Attribute
{
    public string Code { get; set; } = error;
    public string Description { get; set; } = description;
}