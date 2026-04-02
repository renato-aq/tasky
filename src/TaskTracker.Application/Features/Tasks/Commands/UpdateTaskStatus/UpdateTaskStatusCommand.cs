using TaskTracker.Application.Abstractions.CQRS;
using TaskTracker.Domain.Enums;

namespace TaskTracker.Application.Features.Tasks.Commands.UpdateTaskStatus;

public record UpdateTaskStatusCommand(Guid TaskId, TaskItemStatus Status) : ICommand;
