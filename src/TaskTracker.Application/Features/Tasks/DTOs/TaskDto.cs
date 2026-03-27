namespace TaskTracker.Application.Features.Tasks.DTOs;

public record TaskDto(
    Guid Id,
    Guid ProjectId,
    Guid? SprintId,
    Guid? CreatedInCeremonyId,
    string Title,
    string? Description,
    string Status,
    string Priority,
    DateTime? DueDate,
    string CreatedBy,
    Guid? AssignedUserId,
    string? AssignedUserName,
    Guid? AssignedTeamId,
    DateTime CreatedAt,
    DateTime? UpdatedAt);
