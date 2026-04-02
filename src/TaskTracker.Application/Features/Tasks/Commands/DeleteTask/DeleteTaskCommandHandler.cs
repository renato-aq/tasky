using TaskTracker.Application.Abstractions.CQRS;
using TaskTracker.Application.Abstractions.Repositories;

namespace TaskTracker.Application.Features.Tasks.Commands.DeleteTask;

public class DeleteTaskCommandHandler : ICommandHandler<DeleteTaskCommand>
{
    private readonly ITaskRepository _taskRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteTaskCommandHandler(ITaskRepository taskRepository, IUnitOfWork unitOfWork)
    {
        _taskRepository = taskRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task HandleAsync(DeleteTaskCommand command, CancellationToken ct = default)
    {
        var task = await _taskRepository.GetByIdAsync(command.TaskId, ct)
            ?? throw new InvalidOperationException("Task not found.");

        await _taskRepository.RemoveAsync(task, ct);
        await _unitOfWork.CommitAsync(ct);
    }
}
