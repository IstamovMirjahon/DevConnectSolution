using DevConnect.Domain.Entities;

namespace DevConnect.Domain.IRepositories;

public interface IUserTokenRepository : IRepository<UserToken>
{
    Task<UserToken?> GetByRefreshTokenAsync(string refreshToken, CancellationToken ct);
    Task<UserToken?> GetByUserIdAsync(Guid userId, CancellationToken ct);
}