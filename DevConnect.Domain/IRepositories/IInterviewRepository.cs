using DevConnect.Domain.Entities;
using DevConnect.Domain.Enums;
using DevConnect.Domain.Helpers;

namespace DevConnect.Domain.IRepositories;

public interface IInterviewRepository : IRepository<Interview>
{
    Task<List<Interview>> GetByUserIdAsync(Guid userId, CancellationToken ct);
    Task<List<Interview>> GetByLogCheckerIdAsync(Guid logCheckerId, CancellationToken ct);
    
    Task<PagedList<Interview>> GetFilteredInterviewsAsync(
        Guid logCheckerId, 
        List<InterviewStatus>? statuses, 
        int pageNumber, 
        int pageSize, 
        CancellationToken ct);

    Task<(int TotalScheduled, int TotalCompleted, int TotalCancelled, int UniqueUsers)> GetReportStatsAsync(
        Guid logCheckerId, 
        DateTime fromDate, 
        DateTime toDate, 
        CancellationToken ct);

    Task<List<Interview>> GetActiveInterviewsWithDetailsAsync(Guid userId, CancellationToken ct);
}
