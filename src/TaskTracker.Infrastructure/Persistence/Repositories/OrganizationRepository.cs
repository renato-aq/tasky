using Microsoft.EntityFrameworkCore;
using TaskTracker.Application.Abstractions.Repositories;
using TaskTracker.Domain.Entities;
using TaskTracker.Infrastructure.Persistence.Context;

namespace TaskTracker.Infrastructure.Persistence.Repositories;

public class OrganizationRepository : IOrganizationRepository
{
    private readonly AppDbContext _context;

    public OrganizationRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Organization organization, CancellationToken ct = default)
        => await _context.Organizations.AddAsync(organization, ct);

    public async Task<Organization?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await _context.Organizations.FirstOrDefaultAsync(o => o.Id == id, ct);

    public async Task<bool> SlugExistsAsync(string slug, CancellationToken ct = default)
        => await _context.Organizations.AnyAsync(o => o.Slug == slug, ct);
}
