using TaskTracker.Application.Abstractions.CQRS;
using TaskTracker.Application.Features.Tasks.DTOs;

namespace TaskTracker.Application.Features.Tasks.Queries.GetBacklogByProject;

public record GetBacklogByProjectQuery(Guid ProjectId) : IQuery<IEnumerable<TaskDto>>;
