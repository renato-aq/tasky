using TaskTracker.Application.Abstractions.CQRS;
using TaskTracker.Application.Abstractions.Repositories;
using TaskTracker.Application.Features.Tasks.DTOs;

namespace TaskTracker.Application.Features.Tasks.Queries.GetBacklogByProject;

public class GetBacklogByProjectQueryHandler : IQueryHandler<GetBacklogByProjectQuery, IEnumerable<TaskDto>>
{
    private readonly ITaskReadRepository _taskReadRepository;

    public GetBacklogByProjectQueryHandler(ITaskReadRepository taskReadRepository)
    {
        _taskReadRepository = taskReadRepository;
    }

    public Task<IEnumerable<TaskDto>> HandleAsync(GetBacklogByProjectQuery query, CancellationToken ct = default)
        => _taskReadRepository.GetBacklogByProjectAsync(query.ProjectId, ct);
}
