using TaskTracker.Application.Abstractions.CQRS;
using TaskTracker.Application.Features.Notifications.DTOs;

namespace TaskTracker.Application.Features.Notifications.Queries.GetMyNotifications;

public record GetMyNotificationsQuery(Guid UserId) : IQuery<IEnumerable<NotificationDto>>;
