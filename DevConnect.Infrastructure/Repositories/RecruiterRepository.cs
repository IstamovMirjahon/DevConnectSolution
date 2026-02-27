using DevConnect.Domain.Entities;
using DevConnect.Domain.IRepositories;
using DevConnect.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace DevConnect.Infrastructure.Repositories;

public class RecruiterRepository(DefaultContext context) : IRecruiterRepository
{
    public async Task AddAsync(Recruiter recruiter, CancellationToken ct = default)
    {
        await context.Set<Recruiter>().AddAsync(recruiter, ct);
    }

    public async Task<Recruiter?> GetByUserIdAsync(Guid userId, CancellationToken ct = default)
    {
        return await context.Set<Recruiter>()
            .FirstOrDefaultAsync(x => x.UserId == userId, ct);
    }

    public async Task<Recruiter?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await context.Set<Recruiter>()
            .FirstOrDefaultAsync(x => x.Id == id, ct);
    }

    public async Task<bool> ExistsByUserIdAsync(Guid userId, CancellationToken ct = default)
    {
        return await context.Set<Recruiter>()
            .AnyAsync(x => x.UserId == userId, ct);
    }

    public async Task SaveChangesAsync(CancellationToken ct = default)
    {
        await context.SaveChangesAsync(ct);
    }
}
