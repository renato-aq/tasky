using TaskTracker.Application.Abstractions.CQRS;
using TaskTracker.Application.Abstractions.Repositories;
using TaskTracker.Application.Features.Tasks.DTOs;
using TaskTracker.Domain.Entities;

namespace TaskTracker.Application.Features.Tasks.Commands.CreateTask;

public class CreateTaskCommandHandler : ICommandHandler<CreateTaskCommand, TaskDto>
{
    private readonly ISprintRepository _sprintRepository;
    private readonly ITaskRepository _taskRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateTaskCommandHandler(
        ISprintRepository sprintRepository,
        ITaskRepository taskRepository,
        IUnitOfWork unitOfWork)
    {
        _sprintRepository = sprintRepository;
        _taskRepository = taskRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<TaskDto> HandleAsync(CreateTaskCommand command, CancellationToken ct = default)
    {
        var task = TaskItem.Create(
            command.ProjectId,
            command.Title,
            command.CreatedBy,
            command.Description,
            command.Priority,
            command.DueDate,
            sprintId: null,
            command.AssignedUserId,
            command.AssignedTeamId);

        if (command.SprintId.HasValue)
        {
            var sprint = await _sprintRepository.GetByIdAsync(command.SprintId.Value, ct)
                ?? throw new InvalidOperationException("Sprint not found.");

            if (sprint.ProjectId != command.ProjectId)
                throw new InvalidOperationException("Sprint does not belong to the specified project.");

            task.AssignToSprint(command.SprintId.Value, command.CeremonyId);
        }

        await _taskRepository.AddAsync(task, ct);
        await _unitOfWork.CommitAsync(ct);

        return new TaskDto(
            task.Id, task.ProjectId, task.SprintId, task.CreatedInCeremonyId,
            task.Title, task.Description,
            task.Status.ToString().ToLower(), task.Priority.ToString().ToLower(),
            task.DueDate, task.CreatedBy,
            task.AssignedUserId, null, task.AssignedTeamId,
            task.CreatedAt, task.UpdatedAt);
    }
}
