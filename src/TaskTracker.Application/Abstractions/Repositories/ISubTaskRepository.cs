using TaskTracker.Domain.Entities;

namespace TaskTracker.Application.Abstractions.Repositories;

public interface ISubTaskRepository
{
    Task<SubTaskItem?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<IReadOnlyList<SubTaskItem>> GetByTaskIdAsync(Guid taskId, CancellationToken ct = default);
    Task AddAsync(SubTaskItem subTask, CancellationToken ct = default);
    Task UpdateAsync(SubTaskItem subTask, CancellationToken ct = default);
    Task RemoveAsync(SubTaskItem subTask, CancellationToken ct = default);
}
