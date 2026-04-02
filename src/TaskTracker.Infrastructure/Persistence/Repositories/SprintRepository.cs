using Microsoft.EntityFrameworkCore;
using TaskTracker.Application.Abstractions.Repositories;
using TaskTracker.Domain.Entities;
using TaskTracker.Infrastructure.Persistence.Context;

namespace TaskTracker.Infrastructure.Persistence.Repositories;

public class SprintRepository : ISprintRepository
{
    private readonly AppDbContext _ctx;

    public SprintRepository(AppDbContext ctx)
    {
        _ctx = ctx;
    }

    public async Task AddAsync(Sprint sprint, CancellationToken ct = default)
        => await _ctx.Sprints.AddAsync(sprint, ct);

    public async Task AddCeremonyAsync(SprintCeremony ceremony, CancellationToken ct = default)
        => await _ctx.SprintCeremonies.AddAsync(ceremony, ct);

    public async Task<Sprint?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await _ctx.Sprints.FindAsync([id], ct);

    public async Task<Sprint?> GetByIdWithCeremoniesAsync(Guid id, CancellationToken ct = default)
        => await _ctx.Sprints
            .Include(s => s.Ceremonies)
            .FirstOrDefaultAsync(s => s.Id == id, ct);
}
