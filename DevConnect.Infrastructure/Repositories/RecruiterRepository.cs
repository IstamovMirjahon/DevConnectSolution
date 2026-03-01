using DevConnect.Domain.Entities;
using DevConnect.Domain.IRepositories;
using DevConnect.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace DevConnect.Infrastructure.Repositories;

public class RecruiterRepository(DefaultContext context) : Repository<Recruiter>(context), IRecruiterRepository
{
    public async Task<Recruiter?> GetByUserIdAsync(Guid userId, CancellationToken ct = default)
    {
        return await DbSet
            .FirstOrDefaultAsync(x => x.UserId == userId, ct);
    }

    public async Task<bool> ExistsByUserIdAsync(Guid userId, CancellationToken ct = default)
    {
        return await DbSet
            .AnyAsync(x => x.UserId == userId, ct);
    }
}
