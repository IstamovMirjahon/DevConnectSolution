using DevConnect.Application.Models.LogChecker;
using DevConnect.Domain.Enums;
using DevConnect.Domain.Helpers;

namespace DevConnect.Application.Interfaces;

public interface ILogCheckerDeveloperService
{
    Task<Result<List<LogCheckerUserResponse>>> GetDevelopersAsync(UserType? type, CancellationToken ct);
}
