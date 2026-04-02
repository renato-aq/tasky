using TaskTracker.Application.Abstractions.CQRS;
using TaskTracker.Application.Abstractions.Repositories;

namespace TaskTracker.Application.Features.Notifications.Commands.MarkAsRead;

public class MarkAsReadCommandHandler : ICommandHandler<MarkAsReadCommand>
{
    private readonly INotificationRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public MarkAsReadCommandHandler(INotificationRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task HandleAsync(MarkAsReadCommand command, CancellationToken ct = default)
    {
        var notification = await _repository.GetByIdAsync(command.NotificationId, ct)
            ?? throw new InvalidOperationException("Notification not found.");

        if (notification.UserId != command.UserId)
            throw new InvalidOperationException("Access denied.");

        notification.MarkAsRead();
        await _unitOfWork.CommitAsync(ct);
    }
}
