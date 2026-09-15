using TaskTracker.Domain.Entities;

namespace TaskTracker.Application.Abstractions.Repositories;

public interface ITeamRepository
{
    Task AddAsync(Team team, CancellationToken ct = default);
    Task<Team?> GetByIdWithMembersAsync(Guid id, CancellationToken ct = default);
}
