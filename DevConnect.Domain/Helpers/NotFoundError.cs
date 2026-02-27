namespace DevConnect.Domain.Helpers;

public class NotFoundError : Error
{
    public NotFoundError(string code, string message) : base(code, message)
    {
    }
}
