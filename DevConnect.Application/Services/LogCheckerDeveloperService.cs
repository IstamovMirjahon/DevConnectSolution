using DevConnect.Application.Interfaces;
using DevConnect.Application.Models.Interviews;
using DevConnect.Application.Models.LogChecker;
using DevConnect.Domain.Entities;
using DevConnect.Domain.Enums;
using DevConnect.Domain.Helpers;
using DevConnect.Domain.IRepositories;

namespace DevConnect.Application.Services;

public class LogCheckerDeveloperService(
    IUserRepository userRepository, 
    IInterviewRepository interviewRepository,
    IUnitOfWork unitOfWork) : ILogCheckerDeveloperService
{
    public async Task<Result<List<LogCheckerUserResponse>>> GetDevelopersAsync(UserType? type, Profession? profession, CancellationToken ct)
    {
        var users = await userRepository.GetByRoleAndTypeAsync(Role.Developer, type, profession, ct);

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

    public async Task<Result<bool>> ScheduleInterviewAsync(Guid logCheckerId, ScheduleInterviewRequest request, CancellationToken ct)
    {
        var user = await userRepository.GetByIdAsync(request.UserId, ct);
        if (user == null || user.Role != Role.Developer)
        {
            return Result<bool>.Fail(new NotFoundError("User.NotFound", "Developer user not found."));
        }

        var interview = new Interview(
            request.UserId,
            logCheckerId,
            request.InterviewDate,
            request.Notes,
            request.MeetingLink);

        await interviewRepository.AddAsync(interview, ct);
        await unitOfWork.SaveChangesAsync(ct);

        return Result<bool>.Success(true);
    }

    public async Task<Result<PagedList<InterviewResponse>>> GetInterviewsAsync(Guid logCheckerId, InterviewFilterRequest request, CancellationToken ct)
    {
        var pagedList = await interviewRepository.GetFilteredInterviewsAsync(
            logCheckerId,
            request.Statuses,
            request.Page,
            request.PageSize,
            ct);

        var responseItems = pagedList.Items.Select(x => new InterviewResponse
        {
            Id = x.Id,
            UserId = x.UserId,
            LogCheckerId = x.LogCheckerId,
            InterviewDate = x.InterviewDate,
            Status = x.Status,
            Notes = x.Notes,
            MeetingLink = x.MeetingLink,
            Score = x.Score,
            ResultNote = x.ResultNote,
            CreatedAt = x.CreatedAt,
            UpdatedAt = x.UpdatedAt,
            FullName = x.User?.FullName ?? string.Empty,
            Title = x.User?.Title,
            ExperienceLevel = x.User?.ExperienceLevel,
            UserType = x.User?.Type ?? UserType.Unconfirmed
        }).ToList(); // Evaluate list in memory 

        var pagedResponse = new PagedList<InterviewResponse>(
            responseItems,
            pagedList.TotalCount,
            pagedList.CurrentPage,
            pagedList.PageSize);

        return Result<PagedList<InterviewResponse>>.Success(pagedResponse);
    }

    public async Task<Result<bool>> FinishInterviewAsync(Guid logCheckerId, Guid interviewId, FinishDeveloperInterviewRequest request, CancellationToken ct)
    {
        var interview = await interviewRepository.GetByIdAsync(interviewId, ct);
        if (interview == null || interview.LogCheckerId != logCheckerId)
        {
            return Result<bool>.Fail(new Error("Interview.NotFound", "Interview not found or unauthorized"));
        }

        interview.Complete(request.Score, request.ResultNote);
        
        var user = await userRepository.GetByIdAsync(interview.UserId, ct);
        if (user != null)
        {
            user.SetUserType(request.UserType);
        }

        await unitOfWork.SaveChangesAsync(ct);
        return Result<bool>.Success(true);
    }

    public async Task<Result<ReportResponse>> GetReportAsync(Guid logCheckerId, TimePeriod timePeriod, CancellationToken ct)
    {
        var toDate = DateTime.UtcNow;
        var fromDate = timePeriod switch
        {
            TimePeriod.Weekly => toDate.AddDays(-7),
            TimePeriod.Monthly => toDate.AddMonths(-1),
            TimePeriod.Yearly => toDate.AddYears(-1),
            _ => toDate.AddDays(-7)
        };

        var stats = await interviewRepository.GetReportStatsAsync(logCheckerId, fromDate, toDate, ct);

        var report = new ReportResponse
        {
            TotalScheduled = stats.TotalScheduled,
            TotalCompleted = stats.TotalCompleted,
            TotalCancelled = stats.TotalCancelled,
            UniqueUsersInterviewed = stats.UniqueUsers
        };

        return Result<ReportResponse>.Success(report);
    }
}
