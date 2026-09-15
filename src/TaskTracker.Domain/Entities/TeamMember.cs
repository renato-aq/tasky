using TaskTracker.Domain.Enums;

namespace TaskTracker.Domain.Entities;

public class TeamMember
{
    public Guid TeamId { get; private set; }
    public Guid UserId { get; private set; }
    public TeamRole Role { get; private set; }

    public Team Team { get; private set; } = null!;
    public ApplicationUser User { get; private set; } = null!;

    private TeamMember() { }

    public static TeamMember Create(Guid teamId, Guid userId, TeamRole role)
        => new() { TeamId = teamId, UserId = userId, Role = role };
}
