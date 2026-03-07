using DevConnect.Application.Models.LogChecker;
using DevConnect.Domain.Enums;
using DevConnect.Domain.Helpers;

namespace DevConnect.Application.Interfaces;

public interface ILogCheckerCompanyService
{
    Task<Result<List<LogCheckerUserResponse>>> GetRecruitersAsync(UserType? type, CancellationToken ct);
}
