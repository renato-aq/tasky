using Microsoft.EntityFrameworkCore;
using TaskTracker.Application.Abstractions.Repositories;
using TaskTracker.Domain.Entities;
using TaskTracker.Infrastructure.Persistence.Context;

namespace TaskTracker.Infrastructure.Persistence.Repositories;

public class ProjectRepository : IProjectRepository
{
    private readonly AppDbContext _context;

    public ProjectRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Project project, CancellationToken ct = default)
        => await _context.Projects.AddAsync(project, ct);

    public async Task<Project?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await _context.Projects.FindAsync([id], ct);
}
