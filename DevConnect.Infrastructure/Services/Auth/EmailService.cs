using DevConnect.Application.Interfaces;
using DevConnect.Infrastructure.Options;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;

namespace DevConnect.Infrastructure.Services.Auth;

public class EmailService : IEmailService
{
    private readonly EmailSettings _settings;
    private readonly IMemoryCache _cache;

    public EmailService(
        IOptions<EmailSettings> settings,
        IMemoryCache cache)
    {
        _settings = settings.Value;
        _cache = cache;
    }

    // ================= EMAIL CONFIRM =================

    public async Task SendVerificationCodeAsync(string email)
    {
        var code = GenerateCode();

        _cache.Set($"verify_{email}", code, TimeSpan.FromMinutes(2));

        await SendEmailAsync(email, "Email Verification",
            $"Your verification code is: {code}");
    }

    public bool VerifyCode(string email, string code)
    {
        return VerifyInternal($"verify_{email}", code);
    }

    // ================= RESET PASSWORD =================

    public async Task SendResetPasswordCodeAsync(string email)
    {
        var code = GenerateCode();

        _cache.Set($"reset_{email}", code, TimeSpan.FromMinutes(2));

        await SendEmailAsync(email, "Reset Password",
            $"Your reset password code is: {code}");
    }

    public bool VerifyResetPasswordCode(string email, string code)
    {
        return VerifyInternal($"reset_{email}", code);
    }

    // ================= INTERNAL =================

    private bool VerifyInternal(string key, string code)
    {
        if (_cache.TryGetValue(key, out string? cachedCode))
        {
            if (cachedCode == code)
            {
                _cache.Remove(key);
                return true;
            }
        }
        return false;
    }

    private async Task SendEmailAsync(string email, string subject, string body)
    {
        var message = new MailMessage
        {
            From = new MailAddress(_settings.SenderEmail, _settings.DisplayName),
            Subject = subject,
            Body = body,
            IsBodyHtml = false
        };

        message.To.Add(email);

        using var smtp = new SmtpClient(_settings.Host, _settings.Port)
        {
            Credentials = new NetworkCredential(
                _settings.SenderEmail,
                _settings.Password),
            EnableSsl = true
        };

        await smtp.SendMailAsync(message);
    }

    private string GenerateCode()
    {
        return RandomNumberGenerator
            .GetInt32(100000, 999999)
            .ToString();
    }
}