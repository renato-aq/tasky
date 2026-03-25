using TaskTracker.Domain.Common;

namespace TaskTracker.Domain.Entities;

public class Organization : Entity<Guid>
{
    public string Name { get; private set; } = string.Empty;
    public string Slug { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; }

    private Organization() { }

    public static Organization Create(string name, string slug)
        => new()
        {
            Id = Guid.NewGuid(),
            Name = name,
            Slug = slug,
            CreatedAt = DateTime.UtcNow
        };
}
