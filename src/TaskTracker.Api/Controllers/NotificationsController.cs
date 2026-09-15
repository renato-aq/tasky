using Microsoft.AspNetCore.Mvc;
using TaskTracker.Api.Models;
using TaskTracker.Application.Abstractions.CQRS;
using TaskTracker.Application.Features.Notifications.Commands.MarkAllAsRead;
using TaskTracker.Application.Features.Notifications.Commands.MarkAsRead;
using TaskTracker.Application.Features.Notifications.DTOs;
using TaskTracker.Application.Features.Notifications.Queries.GetMyNotifications;

namespace TaskTracker.Api.Controllers;

[Route("api/notifications")]
public class NotificationsController : BaseApiController
{
    public NotificationsController(IDispatcher dispatcher) : base(dispatcher) { }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<NotificationDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMyNotifications(CancellationToken ct)
    {
        var result = await Dispatcher.QueryAsync<GetMyNotificationsQuery, IEnumerable<NotificationDto>>(
            new GetMyNotificationsQuery(CurrentUserId), ct);
        return Ok(ApiResponse<IEnumerable<NotificationDto>>.Ok(result));
    }

    [HttpPatch("{id:guid}/read")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> MarkAsRead(Guid id, CancellationToken ct)
    {
        await Dispatcher.SendAsync(new MarkAsReadCommand(id, CurrentUserId), ct);
        return NoContent();
    }

    [HttpPost("read-all")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> MarkAllAsRead(CancellationToken ct)
    {
        await Dispatcher.SendAsync(new MarkAllAsReadCommand(CurrentUserId), ct);
        return NoContent();
    }
}
