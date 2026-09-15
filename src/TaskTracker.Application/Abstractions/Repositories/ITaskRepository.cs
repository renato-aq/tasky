using TaskTracker.Domain.Entities;

namespace TaskTracker.Application.Abstractions.Repositories;

public interface ITaskRepository
{
    Task<TaskItem?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<IReadOnlyList<TaskItem>> GetByIdsAsync(IReadOnlyList<Guid> ids, CancellationToken ct = default);
    Task AddAsync(TaskItem task, CancellationToken ct = default);
    Task UpdateAsync(TaskItem task, CancellationToken ct = default);
    Task RemoveAsync(TaskItem task, CancellationToken ct = default);
}
