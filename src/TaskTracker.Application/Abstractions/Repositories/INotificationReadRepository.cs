using TaskTracker.Application.Features.Notifications.DTOs;

namespace TaskTracker.Application.Abstractions.Repositories;

public interface INotificationReadRepository
{
    Task<IEnumerable<NotificationDto>> GetByUserAsync(Guid userId, CancellationToken ct = default);
}
