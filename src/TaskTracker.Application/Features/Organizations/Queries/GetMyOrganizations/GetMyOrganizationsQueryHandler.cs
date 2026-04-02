using TaskTracker.Application.Abstractions.CQRS;
using TaskTracker.Application.Abstractions.Repositories;
using TaskTracker.Application.Features.Organizations.DTOs;

namespace TaskTracker.Application.Features.Organizations.Queries.GetMyOrganizations;

public class GetMyOrganizationsQueryHandler : IQueryHandler<GetMyOrganizationsQuery, IEnumerable<OrganizationDto>>
{
    private readonly IOrganizationReadRepository _readRepository;

    public GetMyOrganizationsQueryHandler(IOrganizationReadRepository readRepository)
    {
        _readRepository = readRepository;
    }

    public Task<IEnumerable<OrganizationDto>> HandleAsync(GetMyOrganizationsQuery query, CancellationToken ct = default)
        => _readRepository.GetByUserAsync(query.UserId, ct);
}
