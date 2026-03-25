using TaskTracker.Application.Features.Organizations.DTOs;

namespace TaskTracker.Application.Abstractions.Repositories;

public interface IOrganizationReadRepository
{
    Task<OrganizationDto?> GetByIdAsync(Guid id, CancellationToken ct = default);
}
