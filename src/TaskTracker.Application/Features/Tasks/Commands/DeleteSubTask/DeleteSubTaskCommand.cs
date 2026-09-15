using TaskTracker.Application.Abstractions.CQRS;

namespace TaskTracker.Application.Features.Tasks.Commands.DeleteSubTask;

public record DeleteSubTaskCommand(Guid TaskId, Guid SubTaskId) : ICommand;
