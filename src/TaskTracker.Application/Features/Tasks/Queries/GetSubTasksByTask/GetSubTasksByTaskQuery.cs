using TaskTracker.Application.Abstractions.CQRS;
using TaskTracker.Application.Features.Tasks.DTOs;

namespace TaskTracker.Application.Features.Tasks.Queries.GetSubTasksByTask;

public record GetSubTasksByTaskQuery(Guid TaskId) : IQuery<IEnumerable<SubTaskDto>>;
