using DevConnect.Domain.Entities;

namespace DevConnect.Domain.IRepositories;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email, CancellationToken ct);
    Task AddAsync(User user, CancellationToken ct);
    Task<bool> ExistsByEmailAsync(string email, CancellationToken ct);
}