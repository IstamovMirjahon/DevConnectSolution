using DevConnect.Application.Models.Interviews;
using DevConnect.Application.Models.LogChecker;
using DevConnect.Domain.Enums;
using DevConnect.Domain.Helpers;

namespace DevConnect.Application.Interfaces;

public interface ILogCheckerDeveloperService
{
    Task<Result<List<LogCheckerUserResponse>>> GetDevelopersAsync(UserType? type, Profession? profession, CancellationToken ct);
    Task<Result<bool>> ScheduleInterviewAsync(Guid logCheckerId, ScheduleInterviewRequest request, CancellationToken ct);
    Task<Result<PagedList<InterviewResponse>>> GetInterviewsAsync(Guid logCheckerId, InterviewFilterRequest request, CancellationToken ct);
    Task<Result<bool>> FinishInterviewAsync(Guid logCheckerId, Guid interviewId, FinishDeveloperInterviewRequest request, CancellationToken ct);
    Task<Result<ReportResponse>> GetReportAsync(Guid logCheckerId, TimePeriod timePeriod, CancellationToken ct);
}
