using TaskTracker.Application.Abstractions.CQRS;
using TaskTracker.Application.Features.Tasks.DTOs;
using TaskTracker.Domain.Enums;

namespace TaskTracker.Application.Features.Tasks.Commands.CreateTask;

public record CreateTaskCommand(
    Guid ProjectId,
    string Title,
    string CreatedBy,
    string? Description,
    TaskPriority Priority,
    DateTime? DueDate,
    Guid? SprintId,
    Guid? CeremonyId,
    Guid? AssignedUserId,
    Guid? AssignedTeamId
) : ICommand<TaskDto>;
