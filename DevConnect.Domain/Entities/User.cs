using DevConnect.Domain.Enums;

namespace DevConnect.Domain.Entities;

public class User : BaseEntity
{
    public string FullName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    public string? ImageUrl { get; set; }

    public string? PortfolioUrl { get; set; }

    public Role Role { get; set; }

    public Profession? Profession { get; set; }

    public UserType Type { get; set; } = UserType.Unconfirmed;

    public virtual Recruiter? Recruiter { get; set; }

    private User() { } // EF

    public User(
        string fullName,
        string email,
        string passwordHash,
        Role role)
    {
        FullName = fullName;
        Email = email.ToLower();
        PasswordHash = passwordHash;
        Role = role;
        State = State.Active;
        Type = UserType.Unconfirmed;
    }

    public void ConfirmEmail()
    {
        Type = UserType.Confirmed;
        UpdatedAt = DateTime.UtcNow;
    }

    public void ChangePassword(string newHash)
    {
        PasswordHash = newHash;
        UpdatedAt = DateTime.UtcNow;
    }
}