using DevConnect.Domain.Entities;
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
}
