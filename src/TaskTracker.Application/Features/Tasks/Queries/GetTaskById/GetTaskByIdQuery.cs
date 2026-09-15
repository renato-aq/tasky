using TaskTracker.Application.Abstractions.CQRS;
using TaskTracker.Application.Features.Tasks.DTOs;

namespace TaskTracker.Application.Features.Tasks.Queries.GetTaskById;

public record GetTaskByIdQuery(Guid TaskId) : IQuery<TaskDto?>;
