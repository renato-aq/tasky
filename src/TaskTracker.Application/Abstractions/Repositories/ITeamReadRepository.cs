using TaskTracker.Application.Features.Teams.DTOs;

namespace TaskTracker.Application.Abstractions.Repositories;

public interface ITeamReadRepository
{
    Task<IEnumerable<TeamDto>> GetByOrganizationAsync(Guid organizationId, CancellationToken ct = default);
    Task<TeamDetailDto?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<IEnumerable<TeamDto>> GetByUserAsync(Guid userId, CancellationToken ct = default);
}
