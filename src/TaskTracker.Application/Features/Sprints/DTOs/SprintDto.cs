namespace TaskTracker.Application.Features.Sprints.DTOs;

public record SprintDto(
    Guid Id,
    Guid ProjectId,
    string Name,
    string? Goal,
    int DurationDays,
    DateTime? StartDate,
    DateTime? EndDate,
    string Status,
    DateTime CreatedAt);
