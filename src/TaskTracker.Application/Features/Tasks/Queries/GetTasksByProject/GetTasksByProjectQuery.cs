using TaskTracker.Application.Abstractions.CQRS;
using TaskTracker.Application.Features.Tasks.DTOs;

namespace TaskTracker.Application.Features.Tasks.Queries.GetTasksByProject;

public record GetTasksByProjectQuery(Guid ProjectId) : IQuery<IEnumerable<TaskDto>>;
