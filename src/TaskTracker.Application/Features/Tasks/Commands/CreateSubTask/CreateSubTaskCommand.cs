using TaskTracker.Application.Abstractions.CQRS;
using TaskTracker.Application.Features.Tasks.DTOs;

namespace TaskTracker.Application.Features.Tasks.Commands.CreateSubTask;

public record CreateSubTaskCommand(Guid TaskId, string Title, Guid? AssignedUserId) : ICommand<SubTaskDto>;
