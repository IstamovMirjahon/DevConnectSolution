namespace DevConnect.Domain.Entities;

public class UserToken : BaseEntity
{
    public string AccessToken { get; set; } = null!;
    public string RefreshToken { get; set; } = null!;
    public DateTime AccessTokenExpiration { get; set; }
    public DateTime RefreshTokenExpiration { get; set; }
    public string? PasswordToken { get; set; }
    public DateTime? PasswordTokenExpiration { get; set; }
    public Guid UserId { get; set; }
}