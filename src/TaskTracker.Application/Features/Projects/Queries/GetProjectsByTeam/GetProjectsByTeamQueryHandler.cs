using TaskTracker.Application.Abstractions.CQRS;
using TaskTracker.Application.Abstractions.Repositories;
using TaskTracker.Application.Features.Projects.DTOs;

namespace TaskTracker.Application.Features.Projects.Queries.GetProjectsByTeam;

public class GetProjectsByTeamQueryHandler : IQueryHandler<GetProjectsByTeamQuery, IEnumerable<ProjectDto>>
{
    private readonly IProjectReadRepository _projectReadRepository;

    public GetProjectsByTeamQueryHandler(IProjectReadRepository projectReadRepository)
    {
        _projectReadRepository = projectReadRepository;
    }

    public Task<IEnumerable<ProjectDto>> HandleAsync(GetProjectsByTeamQuery query, CancellationToken ct = default)
        => _projectReadRepository.GetByTeamIdAsync(query.TeamId, ct);
}
