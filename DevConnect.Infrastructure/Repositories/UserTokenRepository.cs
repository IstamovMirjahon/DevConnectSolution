using DevConnect.Domain.Entities;
using DevConnect.Domain.IRepositories;
using DevConnect.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace DevConnect.Infrastructure.Repositories;

public class UserTokenRepository(DefaultContext context) : Repository<UserToken>(context), IUserTokenRepository
{
    public async Task<UserToken?> GetByRefreshTokenAsync(string refreshToken, CancellationToken ct)
    {
        return await DbSet
            .FirstOrDefaultAsync(
                x => x.RefreshToken == refreshToken,
                ct);
    }

    public async Task<UserToken?> GetByUserIdAsync(Guid userId, CancellationToken ct)
    {
        return await DbSet
            .FirstOrDefaultAsync(
                x => x.UserId == userId,
                ct);
    }
}