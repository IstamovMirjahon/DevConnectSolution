using DevConnect.Domain.Enums;

namespace DevConnect.Domain.Entities;

/// <summary>
/// LogCheckerCompany (recruiter bilan) yoki LogCheckerDeveloper (developer bilan)
/// o'tkazadigan intervyu/suhbat yozuvi.
/// </summary>
public class Interview : BaseEntity
{
    public Guid UserId { get; set; }
    public Guid LogCheckerId { get; set; }
    public DateTime InterviewDate { get; set; }
    public InterviewStatus Status { get; set; } = InterviewStatus.Pending;
    public string? Notes { get; set; }
    public string? MeetingLink { get; set; }
    public byte? Score { get; private set; }
    public string? ResultNote { get; private set; }
    public virtual User? User { get; set; }
    public virtual User? LogChecker { get; set; }

    private Interview() { } // EF Core

    public Interview(
        Guid userId,
        Guid logCheckerId,
        DateTime interviewDate,
        string? notes = null,
        string? meetingLink = null)
    {
        UserId = userId;
        LogCheckerId = logCheckerId;
        InterviewDate = interviewDate;
        Notes = notes;
        MeetingLink = meetingLink;
        Status = InterviewStatus.Pending;
        State = Enums.State.Active;
    }

    public void Confirm()
    {
        Status = InterviewStatus.Confirmed;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Cancel()
    {
        Status = InterviewStatus.Cancelled;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Complete(byte? score = null, string? resultNote = null)
    {
        Status = InterviewStatus.Completed;
        Score = score;
        ResultNote = resultNote;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateMeetingLink(string meetingLink)
    {
        MeetingLink = meetingLink;
        UpdatedAt = DateTime.UtcNow;
    }
}
