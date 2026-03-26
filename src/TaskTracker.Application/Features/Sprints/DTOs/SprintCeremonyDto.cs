namespace TaskTracker.Application.Features.Sprints.DTOs;

public record SprintCeremonyDto(
    Guid Id,
    Guid SprintId,
    string Type,
    string? Notes,
    DateTime OccurredAt);
