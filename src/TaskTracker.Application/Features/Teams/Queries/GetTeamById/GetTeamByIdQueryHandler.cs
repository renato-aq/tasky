using TaskTracker.Application.Abstractions.CQRS;
using TaskTracker.Application.Abstractions.Repositories;
using TaskTracker.Application.Features.Teams.DTOs;

namespace TaskTracker.Application.Features.Teams.Queries.GetTeamById;

public class GetTeamByIdQueryHandler : IQueryHandler<GetTeamByIdQuery, TeamDetailDto?>
{
    private readonly ITeamReadRepository _readRepository;

    public GetTeamByIdQueryHandler(ITeamReadRepository readRepository)
    {
        _readRepository = readRepository;
    }

    public Task<TeamDetailDto?> HandleAsync(GetTeamByIdQuery query, CancellationToken ct = default)
        => _readRepository.GetByIdAsync(query.TeamId, ct);
}
