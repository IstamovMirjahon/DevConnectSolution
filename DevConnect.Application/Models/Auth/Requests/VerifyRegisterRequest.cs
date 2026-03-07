namespace DevConnect.Application.Models.Auth.Requests;

public class VerifyRegisterRequest
{
    public string Email { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
}
