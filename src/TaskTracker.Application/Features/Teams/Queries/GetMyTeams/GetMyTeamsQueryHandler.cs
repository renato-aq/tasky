using TaskTracker.Application.Abstractions.CQRS;
using TaskTracker.Application.Abstractions.Repositories;
using TaskTracker.Application.Features.Teams.DTOs;

namespace TaskTracker.Application.Features.Teams.Queries.GetMyTeams;

public class GetMyTeamsQueryHandler : IQueryHandler<GetMyTeamsQuery, IEnumerable<TeamDto>>
{
    private readonly ITeamReadRepository _readRepository;

    public GetMyTeamsQueryHandler(ITeamReadRepository readRepository)
    {
        _readRepository = readRepository;
    }

    public Task<IEnumerable<TeamDto>> HandleAsync(GetMyTeamsQuery query, CancellationToken ct = default)
        => _readRepository.GetByUserAsync(query.UserId, ct);
}
