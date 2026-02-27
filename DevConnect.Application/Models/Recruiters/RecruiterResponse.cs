namespace DevConnect.Application.Models.Recruiters;

public class RecruiterResponse
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string CompanyAddress { get; set; } = string.Empty;
    public string CompanyRole { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
