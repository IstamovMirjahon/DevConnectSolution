using DevConnect.Domain.Helpers;

namespace DevConnect.Application.Interfaces;

public interface IUserNotificationService
{
    Task<Result<List<string>>> GetUserNotificationsAsync(Guid userId, CancellationToken ct);
}
