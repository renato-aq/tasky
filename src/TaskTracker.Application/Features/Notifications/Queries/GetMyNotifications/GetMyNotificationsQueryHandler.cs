using TaskTracker.Application.Abstractions.CQRS;
using TaskTracker.Application.Abstractions.Repositories;
using TaskTracker.Application.Features.Notifications.DTOs;

namespace TaskTracker.Application.Features.Notifications.Queries.GetMyNotifications;

public class GetMyNotificationsQueryHandler : IQueryHandler<GetMyNotificationsQuery, IEnumerable<NotificationDto>>
{
    private readonly INotificationReadRepository _readRepository;

    public GetMyNotificationsQueryHandler(INotificationReadRepository readRepository)
    {
        _readRepository = readRepository;
    }

    public Task<IEnumerable<NotificationDto>> HandleAsync(GetMyNotificationsQuery query, CancellationToken ct = default)
        => _readRepository.GetByUserAsync(query.UserId, ct);
}
