using TaskTracker.Application.Abstractions.CQRS;

namespace TaskTracker.Application.Features.Tasks.Commands.DeleteTask;

public record DeleteTaskCommand(Guid TaskId) : ICommand;
