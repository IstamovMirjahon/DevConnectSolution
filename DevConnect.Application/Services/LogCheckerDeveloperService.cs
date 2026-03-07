using DevConnect.Application.Interfaces;
using DevConnect.Application.Models.LogChecker;
using DevConnect.Domain.Enums;
using DevConnect.Domain.Helpers;
using DevConnect.Domain.IRepositories;

namespace DevConnect.Application.Services;

public class LogCheckerDeveloperService(IUserRepository userRepository) : ILogCheckerDeveloperService
{
    public async Task<Result<List<LogCheckerUserResponse>>> GetDevelopersAsync(UserType? type, CancellationToken ct)
    {
        var users = await userRepository.GetByRoleAndTypeAsync(Role.Developer, type, ct);

        var response = users.Select(u => new LogCheckerUserResponse
        {
            Id = u.Id,
            FullName = u.FullName,
            Email = u.Email,
            TgUsername = u.TgUsername,
            PhoneNumber = u.PhoneNumber,
            ImageUrl = u.ImageUrl,
            PortfolioUrl = u.PortfolioUrl,
            Role = u.Role,
            Profession = u.Profession,
            Type = u.Type,
            State = u.State,
            CreatedAt = u.CreatedAt
        }).ToList();

        return Result<List<LogCheckerUserResponse>>.Success(response);
    }
}
