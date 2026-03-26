using TaskTracker.Application.Abstractions.CQRS;
using TaskTracker.Application.Abstractions.Repositories;
using TaskTracker.Application.Features.Sprints.DTOs;

namespace TaskTracker.Application.Features.Sprints.Queries.GetSprintById;

public class GetSprintByIdQueryHandler : IQueryHandler<GetSprintByIdQuery, SprintDto?>
{
    private readonly ISprintReadRepository _readRepository;

    public GetSprintByIdQueryHandler(ISprintReadRepository readRepository)
    {
        _readRepository = readRepository;
    }

    public async Task<SprintDto?> HandleAsync(GetSprintByIdQuery query, CancellationToken ct = default)
        => await _readRepository.GetByIdAsync(query.SprintId, ct);
}
