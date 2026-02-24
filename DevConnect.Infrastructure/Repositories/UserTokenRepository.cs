using DevConnect.Domain.Entities;
using DevConnect.Domain.IRepositories;
using DevConnect.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace DevConnect.Infrastructure.Repositories;

public class UserTokenRepository(DefaultContext _context) : IUserTokenRepository
{
    public async Task AddAsync(UserToken token, CancellationToken ct)
    {
        await _context.Set<UserToken>()
            .AddAsync(token, ct);
    }

    public async Task<UserToken?> GetByRefreshTokenAsync(string refreshToken, CancellationToken ct)
    {
        return await _context.Set<UserToken>()
            .FirstOrDefaultAsync(
                x => x.RefreshToken == refreshToken,
                ct);
    }

    public async Task<UserToken?> GetByUserIdAsync(Guid userId, CancellationToken ct)
    {
        return await _context.Set<UserToken>()
            .FirstOrDefaultAsync(
                x => x.UserId == userId,
                ct);
    }

    public async Task SaveChangesAsync(CancellationToken ct)
    {
        await _context.SaveChangesAsync(ct);
    }
}