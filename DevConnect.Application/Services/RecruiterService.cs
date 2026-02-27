using DevConnect.Application.Interfaces;
using DevConnect.Application.Models.Recruiters;
using DevConnect.Domain.Entities;
using DevConnect.Domain.Helpers;
using DevConnect.Domain.IRepositories;

namespace DevConnect.Application.Services;

public class RecruiterService(IRecruiterRepository recruiterRepository, IUserRepository userRepository) : IRecruiterService
{
    public async Task<Result<RecruiterResponse>> CreateRecruiterAsync(CreateRecruiterRequest request, CancellationToken ct = default)
    {
        var existing = await recruiterRepository.ExistsByUserIdAsync(request.UserId, ct);
        if (existing)
            return Result<RecruiterResponse>.Fail(new Error("Recruiter profile already exists for this user."));

        var user = await userRepository.GetByIdAsync(request.UserId, ct);
        if (user == null)
            return Result<RecruiterResponse>.Fail(new Error("User not found"));

        var recruiter = new Recruiter
        {
            UserId = request.UserId,
            FullName = request.FullName,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            CompanyAddress = request.CompanyAddress,
            CompanyRole = request.CompanyRole
        };

        await recruiterRepository.AddAsync(recruiter, ct);
        await recruiterRepository.SaveChangesAsync(ct);

        var response = new RecruiterResponse
        {
            Id = recruiter.Id,
            UserId = recruiter.UserId,
            FullName = recruiter.FullName,
            Email = recruiter.Email,
            PhoneNumber = recruiter.PhoneNumber,
            CompanyAddress = recruiter.CompanyAddress,
            CompanyRole = recruiter.CompanyRole,
            CreatedAt = recruiter.CreatedAt
        };

        return Result<RecruiterResponse>.Success(response);
    }

    public async Task<Result<RecruiterResponse>> GetRecruiterAsync(Guid userId, CancellationToken ct = default)
    {
        var recruiter = await recruiterRepository.GetByUserIdAsync(userId, ct);
        if (recruiter == null)
            return Result<RecruiterResponse>.Fail(new Error("Recruiter profile not found."));

        var response = new RecruiterResponse
        {
            Id = recruiter.Id,
            UserId = recruiter.UserId,
            FullName = recruiter.FullName,
            Email = recruiter.Email,
            PhoneNumber = recruiter.PhoneNumber,
            CompanyAddress = recruiter.CompanyAddress,
            CompanyRole = recruiter.CompanyRole,
            CreatedAt = recruiter.CreatedAt
        };

        return Result<RecruiterResponse>.Success(response);
    }
}
