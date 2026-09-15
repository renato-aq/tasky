using TaskTracker.Application.Abstractions.CQRS;
using TaskTracker.Application.Abstractions.Repositories;
using TaskTracker.Application.Features.Teams.DTOs;

namespace TaskTracker.Application.Features.Teams.Queries.GetTeamsByOrganization;

public class GetTeamsByOrganizationQueryHandler : IQueryHandler<GetTeamsByOrganizationQuery, IEnumerable<TeamDto>>
{
    private readonly ITeamReadRepository _readRepository;

    public GetTeamsByOrganizationQueryHandler(ITeamReadRepository readRepository)
    {
        _readRepository = readRepository;
    }

    public Task<IEnumerable<TeamDto>> HandleAsync(GetTeamsByOrganizationQuery query, CancellationToken ct = default)
        => _readRepository.GetByOrganizationAsync(query.OrganizationId, ct);
}
