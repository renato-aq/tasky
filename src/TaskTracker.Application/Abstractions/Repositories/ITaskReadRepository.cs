using TaskTracker.Application.Features.Tasks.DTOs;

namespace TaskTracker.Application.Abstractions.Repositories;

public interface ITaskReadRepository
{
    Task<TaskDto?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<IEnumerable<TaskDto>> GetByProjectAsync(Guid projectId, CancellationToken ct = default);
    Task<IEnumerable<TaskDto>> GetBacklogByProjectAsync(Guid projectId, CancellationToken ct = default);
    Task<IEnumerable<TaskDto>> GetByTeamPoolAsync(Guid teamId, CancellationToken ct = default);
    Task<IEnumerable<Guid>> GetAssignedUsersBySprintAsync(Guid sprintId, CancellationToken ct = default);
}
