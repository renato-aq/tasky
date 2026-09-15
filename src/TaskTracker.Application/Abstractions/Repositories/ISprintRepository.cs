using TaskTracker.Domain.Entities;

namespace TaskTracker.Application.Abstractions.Repositories;

public interface ISprintRepository
{
    Task AddAsync(Sprint sprint, CancellationToken ct = default);
    Task AddCeremonyAsync(SprintCeremony ceremony, CancellationToken ct = default);
    Task<Sprint?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<Sprint?> GetByIdWithCeremoniesAsync(Guid id, CancellationToken ct = default);
}
