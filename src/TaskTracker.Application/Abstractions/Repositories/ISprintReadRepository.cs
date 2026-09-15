using TaskTracker.Application.Features.Sprints.DTOs;

namespace TaskTracker.Application.Abstractions.Repositories;

public interface ISprintReadRepository
{
    Task<IEnumerable<SprintDto>> GetByProjectIdAsync(Guid projectId, CancellationToken ct = default);
    Task<SprintDto?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<IEnumerable<SprintCeremonyDto>> GetCeremoniesAsync(Guid sprintId, CancellationToken ct = default);
    Task<bool> HasActiveSprintAsync(Guid projectId, CancellationToken ct = default);
}
