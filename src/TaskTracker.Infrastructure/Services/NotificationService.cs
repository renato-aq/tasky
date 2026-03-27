using TaskTracker.Application.Abstractions.Repositories;
using TaskTracker.Application.Abstractions.Services;
using TaskTracker.Domain.Entities;

namespace TaskTracker.Infrastructure.Services;

public class NotificationService : INotificationService
{
    private readonly INotificationRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public NotificationService(INotificationRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task NotifyTaskAssignedAsync(Guid userId, Guid taskId, string taskTitle, CancellationToken ct = default)
    {
        var notification = Notification.Create(userId, "task_assigned", new { taskId, taskTitle });
        await _repository.AddAsync(notification, ct);
        await _unitOfWork.CommitAsync(ct);
    }

    public async Task NotifyTaskStatusChangedAsync(Guid userId, Guid taskId, string taskTitle, string newStatus, CancellationToken ct = default)
    {
        var notification = Notification.Create(userId, "task_status_changed", new { taskId, taskTitle, newStatus });
        await _repository.AddAsync(notification, ct);
        await _unitOfWork.CommitAsync(ct);
    }

    public async Task NotifySubtaskAddedAsync(Guid userId, Guid taskId, string taskTitle, string subtaskTitle, CancellationToken ct = default)
    {
        var notification = Notification.Create(userId, "subtask_added", new { taskId, taskTitle, subtaskTitle });
        await _repository.AddAsync(notification, ct);
        await _unitOfWork.CommitAsync(ct);
    }

    public async Task NotifySprintStartedAsync(IEnumerable<Guid> userIds, Guid sprintId, string sprintName, CancellationToken ct = default)
    {
        var notifications = userIds.Select(uid =>
            Notification.Create(uid, "sprint_started", new { sprintId, sprintName }));
        await _repository.AddRangeAsync(notifications, ct);
        await _unitOfWork.CommitAsync(ct);
    }

    public async Task NotifySprintCompletedAsync(IEnumerable<Guid> userIds, Guid sprintId, string sprintName, CancellationToken ct = default)
    {
        var notifications = userIds.Select(uid =>
            Notification.Create(uid, "sprint_completed", new { sprintId, sprintName }));
        await _repository.AddRangeAsync(notifications, ct);
        await _unitOfWork.CommitAsync(ct);
    }
}
