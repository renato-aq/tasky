using TaskTracker.Application.Abstractions.CQRS;
using TaskTracker.Domain.Enums;

namespace TaskTracker.Application.Features.Tasks.Commands.UpdateSubTaskStatus;

public record UpdateSubTaskStatusCommand(Guid SubTaskId, SubTaskStatus Status) : ICommand;
