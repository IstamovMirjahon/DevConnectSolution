namespace DevConnect.Domain.Helpers;

public class UserError(string code, string message) : Error(code, message);
