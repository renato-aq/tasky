using TaskTracker.Application.Features.Projects.DTOs;

namespace TaskTracker.Application.Abstractions.Repositories;

public interface IProjectReadRepository
{
    Task<IEnumerable<ProjectDto>> GetByTeamIdAsync(Guid teamId, CancellationToken ct = default);
    Task<ProjectDto?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<IEnumerable<ProjectDto>> GetByUserAsync(Guid userId, CancellationToken ct = default);
}
