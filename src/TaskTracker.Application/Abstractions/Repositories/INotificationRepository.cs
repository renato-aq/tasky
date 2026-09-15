using TaskTracker.Domain.Entities;

namespace TaskTracker.Application.Abstractions.Repositories;

public interface INotificationRepository
{
    Task AddAsync(Notification notification, CancellationToken ct = default);
    Task AddRangeAsync(IEnumerable<Notification> notifications, CancellationToken ct = default);
    Task<Notification?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task MarkAllAsReadAsync(Guid userId, CancellationToken ct = default);
}
