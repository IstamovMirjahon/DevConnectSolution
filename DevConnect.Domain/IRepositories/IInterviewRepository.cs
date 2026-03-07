using DevConnect.Domain.Entities;

namespace DevConnect.Domain.IRepositories;

public interface IInterviewRepository : IRepository<Interview>
{
    Task<List<Interview>> GetByUserIdAsync(Guid userId, CancellationToken ct);
    Task<List<Interview>> GetByLogCheckerIdAsync(Guid logCheckerId, CancellationToken ct);
}
