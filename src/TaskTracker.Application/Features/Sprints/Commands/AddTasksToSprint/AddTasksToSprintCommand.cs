using TaskTracker.Application.Abstractions.CQRS;

namespace TaskTracker.Application.Features.Sprints.Commands.AddTasksToSprint;

public record AddTasksToSprintCommand(
    Guid SprintId,
    IReadOnlyList<Guid> TaskIds,
    Guid? CeremonyId
) : ICommand;
