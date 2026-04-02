using TaskTracker.Application.Abstractions.CQRS;
using TaskTracker.Application.Abstractions.Repositories;
using TaskTracker.Application.Abstractions.Services;

namespace TaskTracker.Application.Features.Tasks.Commands.UpdateTaskStatus;

public class UpdateTaskStatusCommandHandler : ICommandHandler<UpdateTaskStatusCommand>
{
    private readonly ITaskRepository _taskRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly INotificationService _notificationService;

    public UpdateTaskStatusCommandHandler(
        ITaskRepository taskRepository,
        IUnitOfWork unitOfWork,
        INotificationService notificationService)
    {
        _taskRepository = taskRepository;
        _unitOfWork = unitOfWork;
        _notificationService = notificationService;
    }

    public async Task HandleAsync(UpdateTaskStatusCommand command, CancellationToken ct = default)
    {
        var task = await _taskRepository.GetByIdAsync(command.TaskId, ct)
            ?? throw new InvalidOperationException("Task not found.");

        task.UpdateStatus(command.Status);
        await _unitOfWork.CommitAsync(ct);

        if (task.AssignedUserId.HasValue)
            await _notificationService.NotifyTaskStatusChangedAsync(
                task.AssignedUserId.Value, task.Id, task.Title,
                command.Status.ToString().ToLower(), ct);
    }
}
