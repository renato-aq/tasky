using System.Text.Json;

namespace TaskTracker.Application.Features.Notifications.DTOs;

public record NotificationDto(
    Guid Id,
    Guid UserId,
    string Type,
    JsonElement? Payload,
    DateTime? ReadAt,
    DateTime CreatedAt);
