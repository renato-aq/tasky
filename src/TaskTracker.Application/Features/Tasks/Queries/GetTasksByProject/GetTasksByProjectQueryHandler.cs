using TaskTracker.Application.Abstractions.CQRS;
using TaskTracker.Application.Abstractions.Repositories;
using TaskTracker.Application.Features.Tasks.DTOs;

namespace TaskTracker.Application.Features.Tasks.Queries.GetTasksByProject;

public class GetTasksByProjectQueryHandler : IQueryHandler<GetTasksByProjectQuery, IEnumerable<TaskDto>>
{
    private readonly ITaskReadRepository _taskReadRepository;

    public GetTasksByProjectQueryHandler(ITaskReadRepository taskReadRepository)
    {
        _taskReadRepository = taskReadRepository;
    }

    public Task<IEnumerable<TaskDto>> HandleAsync(GetTasksByProjectQuery query, CancellationToken ct = default)
        => _taskReadRepository.GetByProjectAsync(query.ProjectId, ct);
}
