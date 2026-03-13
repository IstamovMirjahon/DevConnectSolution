using DevConnect.Application.Models.Interviews;
using DevConnect.Application.Models.LogChecker;
using DevConnect.Domain.Enums;
using DevConnect.Domain.Helpers;

namespace DevConnect.Application.Interfaces;

public interface ILogCheckerCompanyService
{
    Task<Result<List<LogCheckerUserResponse>>> GetRecruitersAsync(UserType? type, CancellationToken ct);
    Task<Result<bool>> ScheduleInterviewAsync(Guid logCheckerId, ScheduleInterviewRequest request, CancellationToken ct);
    Task<Result<PagedList<InterviewResponse>>> GetInterviewsAsync(Guid logCheckerId, InterviewFilterRequest request, CancellationToken ct);
    Task<Result<bool>> FinishInterviewAsync(Guid logCheckerId, Guid interviewId, FinishRecruiterInterviewRequest request, CancellationToken ct);
    Task<Result<ReportResponse>> GetReportAsync(Guid logCheckerId, TimePeriod timePeriod, CancellationToken ct);
}
