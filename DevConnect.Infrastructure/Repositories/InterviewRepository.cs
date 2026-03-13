using DevConnect.Domain.Entities;
using DevConnect.Domain.Enums;
using DevConnect.Domain.Helpers;
using DevConnect.Domain.IRepositories;
using DevConnect.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace DevConnect.Infrastructure.Repositories;

public class InterviewRepository(DefaultContext context)
    : Repository<Interview>(context), IInterviewRepository
{
    public async Task<List<Interview>> GetByUserIdAsync(Guid userId, CancellationToken ct)
    {
        return await DbSet
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.InterviewDate)
            .ToListAsync(ct);
    }

    public async Task<List<Interview>> GetByLogCheckerIdAsync(Guid logCheckerId, CancellationToken ct)
    {
        return await DbSet
            .Where(x => x.LogCheckerId == logCheckerId)
            .OrderByDescending(x => x.InterviewDate)
            .ToListAsync(ct);
    }

    public async Task<PagedList<Interview>> GetFilteredInterviewsAsync(
        Guid logCheckerId, 
        List<InterviewStatus>? statuses, 
        int pageNumber, 
        int pageSize, 
        CancellationToken ct)
    {
        var query = DbSet
            .Include(x => x.User)
            .Where(x => x.LogCheckerId == logCheckerId);

        if (statuses != null && statuses.Any())
        {
            query = query.Where(x => statuses.Contains(x.Status));
        }

        query = query.OrderBy(x => x.CreatedAt);

        return await PagedList<Interview>.CreateAsync(query, pageNumber, pageSize, ct);
    }

    public async Task<(int TotalScheduled, int TotalCompleted, int TotalCancelled, int UniqueUsers)> GetReportStatsAsync(
        Guid logCheckerId, 
        DateTime fromDate, 
        DateTime toDate, 
        CancellationToken ct)
    {
        var query = DbSet
            .Where(x => x.LogCheckerId == logCheckerId && x.CreatedAt >= fromDate && x.CreatedAt <= toDate);

        var totalScheduled = await query.CountAsync(x => x.Status == InterviewStatus.Pending, ct);
        var totalCompleted = await query.CountAsync(x => x.Status == InterviewStatus.Completed, ct);
        var totalCancelled = await query.CountAsync(x => x.Status == InterviewStatus.Cancelled, ct);
        var uniqueUsers = await query.Select(x => x.UserId).Distinct().CountAsync(ct);

        return (totalScheduled, totalCompleted, totalCancelled, uniqueUsers);
    }

    public async Task<List<Interview>> GetActiveInterviewsWithDetailsAsync(Guid userId, CancellationToken ct)
    {
        return await DbSet
            .Include(x => x.LogChecker)
            .Where(x => x.UserId == userId && (x.Status == InterviewStatus.Pending || x.Status == InterviewStatus.Confirmed))
            .OrderBy(x => x.InterviewDate)
            .ToListAsync(ct);
    }
}
