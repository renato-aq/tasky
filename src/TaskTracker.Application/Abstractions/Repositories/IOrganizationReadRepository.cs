using TaskTracker.Application.Features.Organizations.DTOs;

namespace TaskTracker.Application.Abstractions.Repositories;

public interface IOrganizationReadRepository
{
    Task<OrganizationDto?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<bool> ExistsAsync(Guid id, CancellationToken ct = default);
    Task<IEnumerable<OrganizationDto>> GetByUserAsync(Guid userId, CancellationToken ct = default);
    Task<IEnumerable<OrganizationMemberDto>> GetMembersAsync(Guid organizationId, CancellationToken ct = default);
}
