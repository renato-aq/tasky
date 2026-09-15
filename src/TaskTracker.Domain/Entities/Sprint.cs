using TaskTracker.Domain.Common;
using TaskTracker.Domain.Enums;

namespace TaskTracker.Domain.Entities;

public class Sprint : Entity<Guid>
{
    public Guid ProjectId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string? Goal { get; private set; }
    public int DurationDays { get; private set; }
    public DateTime? StartDate { get; private set; }
    public DateTime? EndDate { get; private set; }
    public SprintStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public ICollection<SprintCeremony> Ceremonies { get; private set; } = [];

    private Sprint() { }

    public static Sprint Create(Guid projectId, string name, string? goal, int durationDays = 14)
        => new()
        {
            Id = Guid.NewGuid(),
            ProjectId = projectId,
            Name = name,
            Goal = goal,
            DurationDays = durationDays,
            Status = SprintStatus.Planning,
            CreatedAt = DateTime.UtcNow
        };

    public void Start(DateTime startDate)
    {
        if (Status != SprintStatus.Planning)
            throw new InvalidOperationException("Sprint is not in planning status.");

        Status = SprintStatus.Active;
        StartDate = startDate;
        EndDate = startDate.AddDays(DurationDays);
    }

    public void Complete(DateTime endDate)
    {
        if (Status != SprintStatus.Active)
            throw new InvalidOperationException("Sprint is not active.");

        Status = SprintStatus.Completed;
        EndDate = endDate;
    }

    public SprintCeremony AddCeremony(CeremonyType type, string? notes, DateTime occurredAt)
    {
        if (Ceremonies.Any(c => c.Type == type))
            throw new InvalidOperationException("Ceremony of this type already exists for this sprint.");

        var ceremony = SprintCeremony.Create(Id, type, notes, occurredAt);
        Ceremonies.Add(ceremony);
        return ceremony;
    }
}
