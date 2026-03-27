using TaskTracker.Application.Abstractions.CQRS;
using TaskTracker.Application.Abstractions.Repositories;
using TaskTracker.Application.Abstractions.Services;
using TaskTracker.Application.Features.Tasks.DTOs;
using TaskTracker.Domain.Entities;

namespace TaskTracker.Application.Features.Tasks.Commands.CreateSubTask;

public class CreateSubTaskCommandHandler : ICommandHandler<CreateSubTaskCommand, SubTaskDto>
{
    private readonly ISubTaskRepository _subTaskRepository;
    private readonly ITaskRepository _taskRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly INotificationService _notificationService;

    public CreateSubTaskCommandHandler(
        ISubTaskRepository subTaskRepository,
        ITaskRepository taskRepository,
        IUnitOfWork unitOfWork,
        INotificationService notificationService)
    {
        _subTaskRepository = subTaskRepository;
        _taskRepository = taskRepository;
        _unitOfWork = unitOfWork;
        _notificationService = notificationService;
    }

    public async Task<SubTaskDto> HandleAsync(CreateSubTaskCommand command, CancellationToken ct = default)
    {
        var parentTask = await _taskRepository.GetByIdAsync(command.TaskId, ct)
            ?? throw new InvalidOperationException("Task not found.");

        var subTask = SubTaskItem.Create(command.TaskId, command.Title, command.AssignedUserId);

        await _subTaskRepository.AddAsync(subTask, ct);
        await _unitOfWork.CommitAsync(ct);

        if (parentTask.AssignedUserId.HasValue)
            await _notificationService.NotifySubtaskAddedAsync(
                parentTask.AssignedUserId.Value, parentTask.Id, parentTask.Title, subTask.Title, ct);

        return new SubTaskDto(
            subTask.Id, subTask.TaskId, subTask.Title,
            subTask.Status.ToString().ToLower(),
            subTask.AssignedUserId, null,
            subTask.CreatedAt);
    }
}
