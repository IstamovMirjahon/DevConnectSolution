using DevConnect.Domain.Enums;

namespace DevConnect.Domain.Entities;

public class User : BaseEntity
{
    public string FullName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    public string? ImageUrl { get; set; }

    public string? PortfolioUrl { get; set; }

    public string? CvUrl { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public List<string> Skills { get; set; } = [];

    public string? ExperienceLevel { get; set; }

    public string? PhoneNumber { get; set; }

    public string TgUsername { get; set; } = string.Empty;

    public Role Role { get; set; }

    public Profession? Profession { get; set; }

    public UserType Type { get; set; } = UserType.Unconfirmed;

    public virtual Recruiter? Recruiter { get; set; }

    private User() { } // EF

    public User(
        string fullName,
        string email,
        string passwordHash,
        Role role,
        string tgUsername)
    {
        FullName = fullName;
        Email = email.ToLower();
        PasswordHash = passwordHash;
        Role = role;
        TgUsername = tgUsername;
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

    public void SetUserType(UserType newType)
    {
        Type = newType;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateCvProfile(
        string cvUrl,
        string title,
        string description,
        List<string> skills,
        string experienceLevel,
        string? portfolioUrl)
    {
        CvUrl = cvUrl;
        Title = title;
        Description = description;
        Skills = skills;
        ExperienceLevel = experienceLevel;
        if (!string.IsNullOrWhiteSpace(portfolioUrl))
            PortfolioUrl = portfolioUrl;
        UpdatedAt = DateTime.UtcNow;
    }
}