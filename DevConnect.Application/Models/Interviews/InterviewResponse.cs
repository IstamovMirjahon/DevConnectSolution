using DevConnect.Domain.Enums;

namespace DevConnect.Application.Models.Interviews;

public class InterviewResponse
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid LogCheckerId { get; set; }
    public DateTime InterviewDate { get; set; }
    public InterviewStatus Status { get; set; }
    public string? Notes { get; set; }
    public string? MeetingLink { get; set; }
    public byte? Score { get; set; }
    public string? ResultNote { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // User details
    public string FullName { get; set; } = string.Empty;
    public string? Title { get; set; }
    public string? ExperienceLevel { get; set; }
    public UserType UserType { get; set; }
}
