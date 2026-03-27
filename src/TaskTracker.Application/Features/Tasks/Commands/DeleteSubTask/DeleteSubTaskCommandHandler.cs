using TaskTracker.Application.Abstractions.CQRS;
using TaskTracker.Application.Abstractions.Repositories;

namespace TaskTracker.Application.Features.Tasks.Commands.DeleteSubTask;

public class DeleteSubTaskCommandHandler : ICommandHandler<DeleteSubTaskCommand>
{
    private readonly ISubTaskRepository _subTaskRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteSubTaskCommandHandler(ISubTaskRepository subTaskRepository, IUnitOfWork unitOfWork)
    {
        _subTaskRepository = subTaskRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task HandleAsync(DeleteSubTaskCommand command, CancellationToken ct = default)
    {
        var subTask = await _subTaskRepository.GetByIdAsync(command.SubTaskId, ct)
            ?? throw new InvalidOperationException($"SubTask {command.SubTaskId} not found.");

        await _subTaskRepository.RemoveAsync(subTask, ct);
        await _unitOfWork.CommitAsync(ct);
    }
}
