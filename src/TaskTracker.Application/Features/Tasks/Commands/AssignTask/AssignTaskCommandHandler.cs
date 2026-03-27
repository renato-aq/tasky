using TaskTracker.Application.Abstractions.CQRS;
using TaskTracker.Application.Abstractions.Repositories;
using TaskTracker.Application.Abstractions.Services;

namespace TaskTracker.Application.Features.Tasks.Commands.AssignTask;

public class AssignTaskCommandHandler : ICommandHandler<AssignTaskCommand>
{
    private readonly ITaskRepository _taskRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly INotificationService _notificationService;

    public AssignTaskCommandHandler(
        ITaskRepository taskRepository,
        IUnitOfWork unitOfWork,
        INotificationService notificationService)
    {
        _taskRepository = taskRepository;
        _unitOfWork = unitOfWork;
        _notificationService = notificationService;
    }

    public async Task HandleAsync(AssignTaskCommand command, CancellationToken ct = default)
    {
        var task = await _taskRepository.GetByIdAsync(command.TaskId, ct)
            ?? throw new InvalidOperationException("Task not found.");

        task.Assign(command.AssignedUserId, command.AssignedTeamId);
        await _unitOfWork.CommitAsync(ct);

        if (task.AssignedUserId.HasValue)
            await _notificationService.NotifyTaskAssignedAsync(
                task.AssignedUserId.Value, task.Id, task.Title, ct);
    }
}
