using TaskTracker.Application.Abstractions.CQRS;
using TaskTracker.Application.Abstractions.Repositories;
using TaskTracker.Application.Features.Sprints.DTOs;

namespace TaskTracker.Application.Features.Sprints.Queries.GetSprintCeremonies;

public class GetSprintCeremoniesQueryHandler : IQueryHandler<GetSprintCeremoniesQuery, IEnumerable<SprintCeremonyDto>>
{
    private readonly ISprintReadRepository _readRepository;

    public GetSprintCeremoniesQueryHandler(ISprintReadRepository readRepository)
    {
        _readRepository = readRepository;
    }

    public async Task<IEnumerable<SprintCeremonyDto>> HandleAsync(GetSprintCeremoniesQuery query, CancellationToken ct = default)
        => await _readRepository.GetCeremoniesAsync(query.SprintId, ct);
}
