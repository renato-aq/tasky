using System.Text.Json;
using TaskTracker.Domain.Common;

namespace TaskTracker.Domain.Entities;

public class Notification : Entity<Guid>
{
    public Guid UserId { get; private set; }
    public string Type { get; private set; } = string.Empty;
    public string Payload { get; private set; } = "{}";
    public DateTime? ReadAt { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public static Notification Create(Guid userId, string type, object payload)
        => new()
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Type = type,
            Payload = JsonSerializer.Serialize(payload),
            CreatedAt = DateTime.UtcNow
        };

    public void MarkAsRead() => ReadAt = DateTime.UtcNow;
}
