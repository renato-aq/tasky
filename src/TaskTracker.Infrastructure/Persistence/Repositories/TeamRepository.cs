using Microsoft.EntityFrameworkCore;
using TaskTracker.Application.Abstractions.Repositories;
using TaskTracker.Domain.Entities;
using TaskTracker.Infrastructure.Persistence.Context;

namespace TaskTracker.Infrastructure.Persistence.Repositories;

public class TeamRepository : ITeamRepository
{
    private readonly AppDbContext _context;

    public TeamRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Team team, CancellationToken ct = default)
        => await _context.Teams.AddAsync(team, ct);

    public async Task<Team?> GetByIdWithMembersAsync(Guid id, CancellationToken ct = default)
        => await _context.Teams
            .Include(t => t.Members)
            .FirstOrDefaultAsync(t => t.Id == id, ct);
}
