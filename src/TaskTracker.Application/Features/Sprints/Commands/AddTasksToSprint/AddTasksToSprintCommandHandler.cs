using TaskTracker.Application.Abstractions.CQRS;
using TaskTracker.Application.Abstractions.Repositories;

namespace TaskTracker.Application.Features.Sprints.Commands.AddTasksToSprint;

public class AddTasksToSprintCommandHandler : ICommandHandler<AddTasksToSprintCommand>
{
    private readonly ITaskRepository _taskRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddTasksToSprintCommandHandler(ITaskRepository taskRepository, IUnitOfWork unitOfWork)
    {
        _taskRepository = taskRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task HandleAsync(AddTasksToSprintCommand command, CancellationToken ct = default)
    {
        if (command.TaskIds.Count == 0)
            return;

        var tasks = await _taskRepository.GetByIdsAsync(command.TaskIds, ct);

        foreach (var task in tasks)
            task.AssignToSprint(command.SprintId, command.CeremonyId);

        await _unitOfWork.CommitAsync(ct);
    }
}
