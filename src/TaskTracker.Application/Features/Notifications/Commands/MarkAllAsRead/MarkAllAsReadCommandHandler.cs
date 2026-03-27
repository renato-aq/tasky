using TaskTracker.Application.Abstractions.CQRS;
using TaskTracker.Application.Abstractions.Repositories;

namespace TaskTracker.Application.Features.Notifications.Commands.MarkAllAsRead;

public class MarkAllAsReadCommandHandler : ICommandHandler<MarkAllAsReadCommand>
{
    private readonly INotificationRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public MarkAllAsReadCommandHandler(INotificationRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task HandleAsync(MarkAllAsReadCommand command, CancellationToken ct = default)
    {
        await _repository.MarkAllAsReadAsync(command.UserId, ct);
        await _unitOfWork.CommitAsync(ct);
    }
}
