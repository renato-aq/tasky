using TaskTracker.Domain.Common;

namespace TaskTracker.Domain.Entities;

public class Project : Entity<Guid>
{
    public Guid TeamId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? DeletedAt { get; private set; }

    private Project() { }

    public static Project Create(Guid teamId, string name, string? description)
        => new()
        {
            Id = Guid.NewGuid(),
            TeamId = teamId,
            Name = name,
            Description = description,
            CreatedAt = DateTime.UtcNow,
            DeletedAt = null
        };

    public void Update(string name, string? description)
    {
        Name = name;
        Description = description;
    }

    public void Delete() => DeletedAt = DateTime.UtcNow;
}
