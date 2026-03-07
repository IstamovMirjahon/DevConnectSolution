using DevConnect.Domain.Entities;
using DevConnect.Domain.Enums;
using DevConnect.Domain.IRepositories;
using DevConnect.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace DevConnect.Infrastructure.Repositories;

public class UserRepository(DefaultContext context) : Repository<User>(context), IUserRepository
{
    public async Task<User?> GetByEmailAsync(string email, CancellationToken ct)
    {
        return await DbSet
            .FirstOrDefaultAsync(x => x.Email == email, ct);
    }

    public async Task<bool> ExistsByEmailAsync(string email, CancellationToken ct)
    {
        return await DbSet
            .AnyAsync(x => x.Email == email, ct);
    }

    public async Task<List<User>> GetByRoleAndTypeAsync(Role role, UserType? type, CancellationToken ct)
    {
        var query = DbSet.Where(x => x.Role == role);

        if (type.HasValue)
            query = query.Where(x => x.Type == type.Value);

        return await query
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync(ct);
    }

    public async Task UpdateImageUrlAsync(Guid userId, string imageUrl, CancellationToken ct)
    {
        await DbSet
            .Where(x => x.Id == userId)
            .ExecuteUpdateAsync(s => s.SetProperty(u => u.ImageUrl, imageUrl), ct);
    }
}
