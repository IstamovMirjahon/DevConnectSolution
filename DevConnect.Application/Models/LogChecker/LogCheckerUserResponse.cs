using DevConnect.Domain.Enums;

namespace DevConnect.Application.Models.LogChecker;

public class LogCheckerUserResponse
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string TgUsername { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public string? ImageUrl { get; set; }
    public string? PortfolioUrl { get; set; }
    public Role Role { get; set; }
    public Profession? Profession { get; set; }
    public UserType Type { get; set; }
    public State State { get; set; }
    public DateTime CreatedAt { get; set; }
}
