using TaskTracker.Application.Abstractions.CQRS;
using TaskTracker.Application.Abstractions.Repositories;
using TaskTracker.Application.Features.Organizations.DTOs;

namespace TaskTracker.Application.Features.Organizations.Queries.GetOrganizationMembers;

public class GetOrganizationMembersQueryHandler : IQueryHandler<GetOrganizationMembersQuery, IEnumerable<OrganizationMemberDto>>
{
    private readonly IOrganizationReadRepository _readRepository;

    public GetOrganizationMembersQueryHandler(IOrganizationReadRepository readRepository)
    {
        _readRepository = readRepository;
    }

    public async Task<IEnumerable<OrganizationMemberDto>> HandleAsync(GetOrganizationMembersQuery query, CancellationToken ct = default)
    {
        if (!await _readRepository.ExistsAsync(query.OrganizationId, ct))
            throw new KeyNotFoundException($"Organization '{query.OrganizationId}' not found.");

        return await _readRepository.GetMembersAsync(query.OrganizationId, ct);
    }
}
