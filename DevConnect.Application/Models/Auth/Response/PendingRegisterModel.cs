using DevConnect.Domain.Enums;

namespace DevConnect.Application.Models.Auth.Response;

public class PendingRegisterModel
{
    public string FullName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public Role Role { get; set; }
    public string TgUsername { get; set; } = null!;
}
