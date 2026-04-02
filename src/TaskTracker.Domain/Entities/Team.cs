using TaskTracker.Domain.Common;
using TaskTracker.Domain.Enums;

namespace TaskTracker.Domain.Entities;

public class Team : Entity<Guid>
{
    public Guid OrganizationId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; }

    public Organization Organization { get; private set; } = null!;
    public ICollection<TeamMember> Members { get; private set; } = [];

    private Team() { }

    public static Team Create(Guid organizationId, string name)
        => new()
        {
            Id = Guid.NewGuid(),
            OrganizationId = organizationId,
            Name = name,
            CreatedAt = DateTime.UtcNow
        };

    public void Update(string name) => Name = name;

    public void AddMember(Guid userId, TeamRole role)
    {
        if (Members.Any(m => m.UserId == userId))
            throw new InvalidOperationException("User is already a member of this team.");
        Members.Add(TeamMember.Create(Id, userId, role));
    }

    public void RemoveMember(Guid userId)
    {
        var member = Members.FirstOrDefault(m => m.UserId == userId)
            ?? throw new InvalidOperationException("User is not a member of this team.");
        ((List<TeamMember>)Members).Remove(member);
    }
}
