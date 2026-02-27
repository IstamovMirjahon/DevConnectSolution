namespace DevConnect.Domain.Entities;

public class Recruiter : BaseEntity
{
    public Guid UserId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string CompanyAddress { get; set; } = string.Empty;
    public string CompanyRole { get; set; } = string.Empty;

    public virtual User? User { get; set; }
}
