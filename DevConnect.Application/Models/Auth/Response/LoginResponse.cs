using DevConnect.Domain.Enums;

namespace DevConnect.Application.Models.Auth.Response;

public class LoginResponse
{
    public Guid Id { get; set; }
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public Role Role { get; set; }
    public UserType UserType { get; set; }
}
