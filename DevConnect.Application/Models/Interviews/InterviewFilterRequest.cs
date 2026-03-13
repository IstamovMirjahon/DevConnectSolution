using DevConnect.Domain.Enums;

namespace DevConnect.Application.Models.Interviews;

public class InterviewFilterRequest
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public List<InterviewStatus>? Statuses { get; set; }
}
