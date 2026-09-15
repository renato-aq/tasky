using TaskTracker.Application.Abstractions.CQRS;
using TaskTracker.Application.Abstractions.Repositories;

namespace TaskTracker.Application.Features.Sprints.Commands.AddTasksToSprint;

public class AddTasksToSprintCommandHandler : ICommandHandler<AddTasksToSprintCommand>
{
    private readonly ISprintRepository _sprintRepository;
    private readonly ITaskRepository _taskRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddTasksToSprintCommandHandler(
        ISprintRepository sprintRepository,
        ITaskRepository taskRepository,
        IUnitOfWork unitOfWork)
    {
        _sprintRepository = sprintRepository;
        _taskRepository = taskRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task HandleAsync(AddTasksToSprintCommand command, CancellationToken ct = default)
    {
        if (command.TaskIds.Count == 0)
            return;

        var sprint = await _sprintRepository.GetByIdAsync(command.SprintId, ct)
            ?? throw new InvalidOperationException("Sprint not found.");

        var tasks = await _taskRepository.GetByIdsAsync(command.TaskIds, ct);

        if (tasks.Count != command.TaskIds.Count)
            throw new InvalidOperationException("One or more tasks were not found.");

        if (tasks.Any(task => task.ProjectId != sprint.ProjectId))
            throw new InvalidOperationException("All tasks must belong to the same project as the sprint.");

        foreach (var task in tasks)
            task.AssignToSprint(command.SprintId, command.CeremonyId);

        await _unitOfWork.CommitAsync(ct);
    }
}
