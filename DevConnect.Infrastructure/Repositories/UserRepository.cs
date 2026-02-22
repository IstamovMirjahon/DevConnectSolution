using DevConnect.Domain.Entities;
using DevConnect.Domain.IRepositories;
using DevConnect.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace DevConnect.Infrastructure.Repositories;

public class UserRepository(DefaultContext context) : IUserRepository
{
    public async Task AddAsync(User user, CancellationToken ct)
    {
        await context.Set<User>().AddAsync(user, ct);
        await context.SaveChangesAsync(ct);
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken ct)
    {
        return await context.Set<User>()
            .FirstOrDefaultAsync(x => x.Email == email, ct);
    }

    public async Task<bool> ExistsByEmailAsync(string email, CancellationToken ct)
    {
        return await context.Set<User>()
            .AnyAsync(x => x.Email == email, ct);
    }
}