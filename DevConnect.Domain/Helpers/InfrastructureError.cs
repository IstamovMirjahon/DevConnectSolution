namespace DevConnect.Domain.Helpers;

public class InfrastructureError : Error
{
    public InfrastructureError(string code, string message) : base(code, message)
    {
    }
}
