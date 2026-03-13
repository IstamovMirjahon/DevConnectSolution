namespace DevConnect.Application.Models.Interviews;

public class ScheduleInterviewRequest
{
    public Guid UserId { get; set; }
    public DateTime InterviewDate { get; set; }
    public string? Notes { get; set; }
    public string? MeetingLink { get; set; }
}
