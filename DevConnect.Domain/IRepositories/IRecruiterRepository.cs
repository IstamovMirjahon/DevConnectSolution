using DevConnect.Domain.Entities;

namespace DevConnect.Domain.IRepositories;

public interface IRecruiterRepository : IRepository<Recruiter>
{
    Task<Recruiter?> GetByUserIdAsync(Guid userId, CancellationToken ct = default);
    Task<bool> ExistsByUserIdAsync(Guid userId, CancellationToken ct = default);
}
