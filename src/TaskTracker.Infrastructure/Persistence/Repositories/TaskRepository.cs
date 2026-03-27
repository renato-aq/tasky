using Microsoft.EntityFrameworkCore;
using TaskTracker.Application.Abstractions.Repositories;
using TaskTracker.Domain.Entities;
using TaskTracker.Infrastructure.Persistence.Context;

namespace TaskTracker.Infrastructure.Persistence.Repositories;

public class TaskRepository : ITaskRepository
{
    private readonly AppDbContext _ctx;

    public TaskRepository(AppDbContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<TaskItem?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await _ctx.Tasks.FindAsync([id], ct);

    public async Task<IReadOnlyList<TaskItem>> GetByIdsAsync(IReadOnlyList<Guid> ids, CancellationToken ct = default)
        => await _ctx.Tasks.Where(t => ids.Contains(t.Id)).ToListAsync(ct);

    public async Task AddAsync(TaskItem task, CancellationToken ct = default)
        => await _ctx.Tasks.AddAsync(task, ct);

    public Task UpdateAsync(TaskItem task, CancellationToken ct = default)
    {
        _ctx.Tasks.Update(task);
        return Task.CompletedTask;
    }

    public Task RemoveAsync(TaskItem task, CancellationToken ct = default)
    {
        _ctx.Tasks.Remove(task);
        return Task.CompletedTask;
    }
}
