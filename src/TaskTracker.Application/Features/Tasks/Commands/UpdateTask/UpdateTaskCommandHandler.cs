using TaskTracker.Application.Abstractions.CQRS;
using TaskTracker.Application.Abstractions.Repositories;

namespace TaskTracker.Application.Features.Tasks.Commands.UpdateTask;

public class UpdateTaskCommandHandler : ICommandHandler<UpdateTaskCommand>
{
    private readonly ITaskRepository _taskRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateTaskCommandHandler(ITaskRepository taskRepository, IUnitOfWork unitOfWork)
    {
        _taskRepository = taskRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task HandleAsync(UpdateTaskCommand command, CancellationToken ct = default)
    {
        var task = await _taskRepository.GetByIdAsync(command.TaskId, ct)
            ?? throw new InvalidOperationException("Task not found.");

        task.Update(command.Title, command.Description, command.Priority, command.DueDate);
        await _unitOfWork.CommitAsync(ct);
    }
}
