using TaskTracker.Application.Abstractions.CQRS;

namespace TaskTracker.Application.Features.Tasks.Commands.AssignTask;

public record AssignTaskCommand(
    Guid TaskId,
    Guid? AssignedUserId,
    Guid? AssignedTeamId
) : ICommand;
