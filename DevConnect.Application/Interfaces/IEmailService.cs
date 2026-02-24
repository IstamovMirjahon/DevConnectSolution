namespace DevConnect.Application.Interfaces;

public interface IEmailService
{
    Task SendVerificationCodeAsync(string email);
    bool VerifyCode(string email, string code);
    Task SendResetPasswordCodeAsync(string email);
    bool VerifyResetPasswordCode(string email, string code);
}