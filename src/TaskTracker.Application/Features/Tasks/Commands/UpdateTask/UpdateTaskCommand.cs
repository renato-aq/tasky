using TaskTracker.Application.Abstractions.CQRS;
using TaskTracker.Domain.Enums;

namespace TaskTracker.Application.Features.Tasks.Commands.UpdateTask;

public record UpdateTaskCommand(
    Guid TaskId,
    string Title,
    string? Description,
    TaskPriority Priority,
    DateTime? DueDate
) : ICommand;
