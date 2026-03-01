using DevConnect.Domain.Enums;

namespace DevConnect.Application.Models.Users;

public class UserProfileResponse
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public string? PortfolioUrl { get; set; }
    public Role Role { get; set; }
    public Profession? Profession { get; set; }
    public UserType Type { get; set; }
    public State State { get; set; }
}
