using DevConnect.Application.Interfaces;
using DevConnect.Domain.Enums;
using DevConnect.Domain.Helpers;
using DevConnect.Domain.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace DevConnect.Application.Services;

public class UserNotificationService(IInterviewRepository interviewRepository) : IUserNotificationService
{
    public async Task<Result<List<string>>> GetUserNotificationsAsync(Guid userId, CancellationToken ct)
    {
        // Get active interviews (Pending, Confirmed) for the user.
        // Include LogChecker to build the notification string.
        var interviews = await interviewRepository.GetActiveInterviewsWithDetailsAsync(userId, ct);

        var notifications = new List<string>();

        foreach (var interview in interviews)
        {
            var logChecker = interview.LogChecker;
            if (logChecker == null)
                continue;

            var titlePart = !string.IsNullOrWhiteSpace(logChecker.Title) ? logChecker.Title : "Recruiter";
            var expPart = !string.IsNullOrWhiteSpace(logChecker.ExperienceLevel) ? $"{logChecker.ExperienceLevel} " : "";
            
            // Format: "Anvar Fayzullayev Middle Frontend dasturchi , intervyudate 27.09.2026, telegram message(google meet, zoom) orqali"
            var message = $"{logChecker.FullName} {expPart}{titlePart} , intervyudate {interview.InterviewDate:dd.MM.yyyy HH:mm}, {interview.MeetingLink ?? interview.Notes} orqali";

            notifications.Add(message);
        }

        return Result<List<string>>.Success(notifications);
    }
}
