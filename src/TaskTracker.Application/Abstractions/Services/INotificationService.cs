namespace TaskTracker.Application.Abstractions.Services;

public interface INotificationService
{
    Task NotifyTaskAssignedAsync(Guid userId, Guid taskId, string taskTitle, CancellationToken ct = default);
    Task NotifyTaskStatusChangedAsync(Guid userId, Guid taskId, string taskTitle, string newStatus, CancellationToken ct = default);
    Task NotifySubtaskAddedAsync(Guid userId, Guid taskId, string taskTitle, string subtaskTitle, CancellationToken ct = default);
    Task NotifySprintStartedAsync(IEnumerable<Guid> userIds, Guid sprintId, string sprintName, CancellationToken ct = default);
    Task NotifySprintCompletedAsync(IEnumerable<Guid> userIds, Guid sprintId, string sprintName, CancellationToken ct = default);
}
