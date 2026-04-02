using TaskTracker.Domain.Common;
using TaskTracker.Domain.Enums;

namespace TaskTracker.Domain.Entities;

public class SprintCeremony : Entity<Guid>
{
    public Guid SprintId { get; private set; }
    public CeremonyType Type { get; private set; }
    public string? Notes { get; private set; }
    public DateTime OccurredAt { get; private set; }

    public Sprint Sprint { get; private set; } = null!;

    private SprintCeremony() { }

    public static SprintCeremony Create(Guid sprintId, CeremonyType type, string? notes, DateTime occurredAt)
        => new()
        {
            Id = Guid.NewGuid(),
            SprintId = sprintId,
            Type = type,
            Notes = notes,
            OccurredAt = occurredAt
        };
}
