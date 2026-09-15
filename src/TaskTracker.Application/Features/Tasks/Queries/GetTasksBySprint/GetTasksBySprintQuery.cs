using TaskTracker.Application.Abstractions.CQRS;
using TaskTracker.Application.Features.Tasks.DTOs;

namespace TaskTracker.Application.Features.Tasks.Queries.GetTasksBySprint;

public record GetTasksBySprintQuery(Guid SprintId) : IQuery<IEnumerable<TaskDto>>;
