using TaskTracker.Application.Abstractions.CQRS;
using TaskTracker.Application.Abstractions.Repositories;
using TaskTracker.Application.Features.Tasks.DTOs;

namespace TaskTracker.Application.Features.Tasks.Queries.GetTasksBySprint;

public class GetTasksBySprintQueryHandler : IQueryHandler<GetTasksBySprintQuery, IEnumerable<TaskDto>>
{
    private readonly ITaskReadRepository _taskReadRepository;

    public GetTasksBySprintQueryHandler(ITaskReadRepository taskReadRepository)
    {
        _taskReadRepository = taskReadRepository;
    }

    public Task<IEnumerable<TaskDto>> HandleAsync(GetTasksBySprintQuery query, CancellationToken ct = default)
        => _taskReadRepository.GetBySprintAsync(query.SprintId, ct);
}
