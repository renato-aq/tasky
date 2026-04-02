using TaskTracker.Application.Abstractions.CQRS;
using TaskTracker.Application.Abstractions.Repositories;
using TaskTracker.Application.Features.Organizations.DTOs;

namespace TaskTracker.Application.Features.Organizations.Queries.GetOrganizationById;

public class GetOrganizationByIdQueryHandler : IQueryHandler<GetOrganizationByIdQuery, OrganizationDto?>
{
    private readonly IOrganizationReadRepository _readRepository;

    public GetOrganizationByIdQueryHandler(IOrganizationReadRepository readRepository)
    {
        _readRepository = readRepository;
    }

    public Task<OrganizationDto?> HandleAsync(GetOrganizationByIdQuery query, CancellationToken ct = default)
        => _readRepository.GetByIdAsync(query.Id, ct);
}
