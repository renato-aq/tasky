using Microsoft.EntityFrameworkCore;
using TaskTracker.Application.Abstractions.Repositories;
using TaskTracker.Domain.Entities;
using TaskTracker.Infrastructure.Persistence.Context;

namespace TaskTracker.Infrastructure.Persistence.Repositories;

public class NotificationRepository : INotificationRepository
{
    private readonly AppDbContext _context;

    public NotificationRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Notification notification, CancellationToken ct = default)
        => await _context.Notifications.AddAsync(notification, ct);

    public async Task AddRangeAsync(IEnumerable<Notification> notifications, CancellationToken ct = default)
        => await _context.Notifications.AddRangeAsync(notifications, ct);

    public async Task<Notification?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await _context.Notifications.FindAsync([id], ct);

    public async Task MarkAllAsReadAsync(Guid userId, CancellationToken ct = default)
        => await _context.Notifications
            .Where(n => n.UserId == userId && n.ReadAt == null)
            .ExecuteUpdateAsync(s => s.SetProperty(n => n.ReadAt, DateTime.UtcNow), ct);
}
