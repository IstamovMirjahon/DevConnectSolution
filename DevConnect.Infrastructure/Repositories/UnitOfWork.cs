using DevConnect.Domain.IRepositories;
using DevConnect.Infrastructure.Context;

namespace DevConnect.Infrastructure.Repositories;

public class UnitOfWork(DefaultContext context) : IUnitOfWork
{
    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        return await context.SaveChangesAsync(ct);
    }
}
