using DevConnect.Domain.Entities;

namespace DevConnect.Domain.IRepositories;

public interface IRecruiterRepository
{
    Task AddAsync(Recruiter recruiter, CancellationToken ct = default);
    Task<Recruiter?> GetByUserIdAsync(Guid userId, CancellationToken ct = default);
    Task<Recruiter?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<bool> ExistsByUserIdAsync(Guid userId, CancellationToken ct = default);
    Task SaveChangesAsync(CancellationToken ct = default);
}
