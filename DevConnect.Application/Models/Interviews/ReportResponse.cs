namespace DevConnect.Application.Models.Interviews;

public class ReportResponse
{
    public int TotalScheduled { get; set; }
    public int TotalCompleted { get; set; }
    public int TotalCancelled { get; set; }
    public int UniqueUsersInterviewed { get; set; }
}
