using TaskTracker.Application.Abstractions.CQRS;

namespace TaskTracker.Application.Features.Notifications.Commands.MarkAsRead;

public record MarkAsReadCommand(Guid NotificationId, Guid UserId) : ICommand;
