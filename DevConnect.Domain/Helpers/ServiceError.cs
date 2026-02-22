namespace DevConnect.Domain.Helpers;

public class ServiceError(string code, string? message = null) : Error(code, message);