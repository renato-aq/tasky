using TaskTracker.Application.Abstractions.CQRS;
using TaskTracker.Application.Abstractions.Repositories;
using TaskTracker.Application.Features.Sprints.DTOs;

namespace TaskTracker.Application.Features.Sprints.Queries.GetSprintsByProject;

public class GetSprintsByProjectQueryHandler : IQueryHandler<GetSprintsByProjectQuery, IEnumerable<SprintDto>>
{
    private readonly ISprintReadRepository _readRepository;

    public GetSprintsByProjectQueryHandler(ISprintReadRepository readRepository)
    {
        _readRepository = readRepository;
    }

    public async Task<IEnumerable<SprintDto>> HandleAsync(GetSprintsByProjectQuery query, CancellationToken ct = default)
        => await _readRepository.GetByProjectIdAsync(query.ProjectId, ct);
}
