namespace DevConnect.Application.Models.Auth.Requests;

public class ResetPasswordRequest
{
    public string Email { get; set; } = null!;
    public string Code { get; set; } = null!;
    public string NewPassword { get; set; } = null!;
}