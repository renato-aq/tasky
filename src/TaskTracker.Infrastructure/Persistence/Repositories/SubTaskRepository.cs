using Microsoft.EntityFrameworkCore;
using TaskTracker.Application.Abstractions.Repositories;
using TaskTracker.Domain.Entities;
using TaskTracker.Infrastructure.Persistence.Context;

namespace TaskTracker.Infrastructure.Persistence.Repositories;

public class SubTaskRepository : ISubTaskRepository
{
    private readonly AppDbContext _ctx;

    public SubTaskRepository(AppDbContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<SubTaskItem?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await _ctx.SubTasks.FindAsync([id], ct);

    public async Task<IReadOnlyList<SubTaskItem>> GetByTaskIdAsync(Guid taskId, CancellationToken ct = default)
        => await _ctx.SubTasks.Where(s => s.TaskId == taskId).ToListAsync(ct);

    public async Task AddAsync(SubTaskItem subTask, CancellationToken ct = default)
        => await _ctx.SubTasks.AddAsync(subTask, ct);

    public Task UpdateAsync(SubTaskItem subTask, CancellationToken ct = default)
    {
        _ctx.SubTasks.Update(subTask);
        return Task.CompletedTask;
    }

    public Task RemoveAsync(SubTaskItem subTask, CancellationToken ct = default)
    {
        _ctx.SubTasks.Remove(subTask);
        return Task.CompletedTask;
    }
}
