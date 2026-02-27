using DevConnect.Application.Models.Recruiters;
using DevConnect.Domain.Helpers;

namespace DevConnect.Application.Interfaces;

public interface IRecruiterService
{
    Task<Result<RecruiterResponse>> CreateRecruiterAsync(CreateRecruiterRequest request, CancellationToken ct = default);
    Task<Result<RecruiterResponse>> GetRecruiterAsync(Guid userId, CancellationToken ct = default);
}