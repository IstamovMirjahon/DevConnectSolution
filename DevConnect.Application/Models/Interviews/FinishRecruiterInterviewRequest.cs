using DevConnect.Domain.Enums;

namespace DevConnect.Application.Models.Interviews;

public class FinishRecruiterInterviewRequest
{
    public string? ResultNote { get; set; }
    public UserType UserType { get; set; }
}
