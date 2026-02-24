using DevConnect.Domain.Entities;

namespace DevConnect.Domain.IRepositories;

public interface IUserTokenRepository
{
    Task<UserToken?> GetByRefreshTokenAsync(string refreshToken, CancellationToken ct);
    Task<UserToken?> GetByUserIdAsync(Guid userId, CancellationToken ct);
    Task AddAsync(UserToken token, CancellationToken ct);
    Task SaveChangesAsync(CancellationToken ct);
}