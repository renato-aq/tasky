using TaskTracker.Domain.Entities;

namespace TaskTracker.Application.Abstractions.Repositories;

public interface IUserRepository
{
    Task<ApplicationUser?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<ApplicationUser?> GetByEmailAsync(string email, CancellationToken ct = default);
    Task<RefreshToken?> GetRefreshTokenAsync(string token, CancellationToken ct = default);
    Task AddRefreshTokenAsync(RefreshToken refreshToken, CancellationToken ct = default);
    Task RevokeRefreshTokenAsync(string token, CancellationToken ct = default);
    Task SetOrganizationAsync(Guid userId, Guid organizationId, CancellationToken ct = default);
}
