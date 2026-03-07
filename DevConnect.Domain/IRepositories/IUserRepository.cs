using DevConnect.Domain.Entities;
using DevConnect.Domain.Enums;

namespace DevConnect.Domain.IRepositories;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByEmailAsync(string email, CancellationToken ct);
    Task<bool> ExistsByEmailAsync(string email, CancellationToken ct);
    Task<List<User>> GetByRoleAndTypeAsync(Role role, UserType? type, CancellationToken ct);
}