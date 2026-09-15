using TaskTracker.Application.Abstractions.CQRS;

namespace TaskTracker.Application.Features.Notifications.Commands.MarkAllAsRead;

public record MarkAllAsReadCommand(Guid UserId) : ICommand;
