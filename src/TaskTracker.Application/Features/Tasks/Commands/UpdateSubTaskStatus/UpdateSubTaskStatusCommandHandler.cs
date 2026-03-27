using TaskTracker.Application.Abstractions.CQRS;
using TaskTracker.Application.Abstractions.Repositories;

namespace TaskTracker.Application.Features.Tasks.Commands.UpdateSubTaskStatus;

public class UpdateSubTaskStatusCommandHandler : ICommandHandler<UpdateSubTaskStatusCommand>
{
    private readonly ISubTaskRepository _subTaskRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateSubTaskStatusCommandHandler(ISubTaskRepository subTaskRepository, IUnitOfWork unitOfWork)
    {
        _subTaskRepository = subTaskRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task HandleAsync(UpdateSubTaskStatusCommand command, CancellationToken ct = default)
    {
        var subTask = await _subTaskRepository.GetByIdAsync(command.SubTaskId, ct)
            ?? throw new InvalidOperationException($"SubTask {command.SubTaskId} not found.");

        subTask.UpdateStatus(command.Status);
        await _subTaskRepository.UpdateAsync(subTask, ct);
        await _unitOfWork.CommitAsync(ct);
    }
}
