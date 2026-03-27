using TaskTracker.Domain.Common;
using TaskTracker.Domain.Enums;

namespace TaskTracker.Domain.Entities;

public class SubTaskItem : Entity<Guid>
{
    public Guid TaskId { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public SubTaskStatus Status { get; private set; }
    public Guid? AssignedUserId { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    private SubTaskItem() { }

    public static SubTaskItem Create(Guid taskId, string title, Guid? assignedUserId = null)
        => new()
        {
            Id = Guid.NewGuid(),
            TaskId = taskId,
            Title = title,
            Status = SubTaskStatus.Todo,
            AssignedUserId = assignedUserId,
            CreatedAt = DateTime.UtcNow
        };

    public void UpdateStatus(SubTaskStatus status)
    {
        Status = status;
        UpdatedAt = DateTime.UtcNow;
    }
}
