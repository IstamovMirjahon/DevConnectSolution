using DevConnect.Domain.Enums;

namespace DevConnect.Domain.Entities;

public class User : BaseEntity
{
    public string FullName { get; private set; } = string.Empty;

    public string Email { get; private set; } = string.Empty;

    public string PasswordHash { get; private set; } = string.Empty;

    public string? ImageUrl { get; private set; }

    public string? PortfolioUrl { get; private set; }

    public Role Role { get; private set; }

    public Profession Profession { get; private set; }

    public UserType Type { get; private set; } = UserType.Unconfirmed;

    private User() { } // EF

    public User(
        string fullName,
        string email,
        string passwordHash,
        Role role,
        Profession profession)
    {
        FullName = fullName;
        Email = email.ToLower();
        PasswordHash = passwordHash;
        Role = role;
        Profession = profession;
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