using DevConnect.Domain.Enums;

namespace DevConnect.Application.Models.Interviews;

public class FinishDeveloperInterviewRequest
{
    public byte Score { get; set; }
    public string? ResultNote { get; set; }
    public UserType UserType { get; set; }
}
