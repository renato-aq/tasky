namespace TaskTracker.Application.Features.Tasks.DTOs;

public record SubTaskDto(
    Guid Id,
    Guid TaskId,
    string Title,
    string Status,
    Guid? AssignedUserId,
    string? AssignedUserName,
    DateTime CreatedAt);
