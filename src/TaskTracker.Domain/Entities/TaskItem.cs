using TaskTracker.Domain.Common;
using TaskTracker.Domain.Enums;

namespace TaskTracker.Domain.Entities;

public class TaskItem : Entity<Guid>
{
    public Guid ProjectId { get; private set; }
    public Guid? SprintId { get; private set; }
    public Guid? CreatedInCeremonyId { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public TaskItemStatus Status { get; private set; }
    public TaskPriority Priority { get; private set; }
    public DateTime? DueDate { get; private set; }
    public string CreatedBy { get; private set; } = string.Empty;
    public Guid? AssignedUserId { get; private set; }
    public Guid? AssignedTeamId { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    private TaskItem() { }

    public static TaskItem Create(
        Guid projectId,
        string title,
        string createdBy,
        string? description = null,
        TaskPriority priority = TaskPriority.Medium,
        DateTime? dueDate = null,
        Guid? sprintId = null,
        Guid? assignedUserId = null,
        Guid? assignedTeamId = null)
        => new()
        {
            Id = Guid.NewGuid(),
            ProjectId = projectId,
            Title = title,
            Description = description,
            Status = TaskItemStatus.Backlog,
            Priority = priority,
            DueDate = dueDate,
            CreatedBy = createdBy,
            SprintId = sprintId,
            AssignedUserId = assignedUserId,
            AssignedTeamId = assignedTeamId,
            CreatedAt = DateTime.UtcNow
        };

    public void Update(string title, string? description, TaskPriority priority, DateTime? dueDate)
    {
        Title = title;
        Description = description;
        Priority = priority;
        DueDate = dueDate;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateStatus(TaskItemStatus status)
    {
        Status = status;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Assign(Guid? assignedUserId, Guid? assignedTeamId)
    {
        AssignedUserId = assignedUserId;
        AssignedTeamId = assignedTeamId;
        UpdatedAt = DateTime.UtcNow;
    }

    public void AssignToSprint(Guid sprintId, Guid? ceremonyId = null)
    {
        SprintId = sprintId;
        CreatedInCeremonyId = ceremonyId;
        UpdatedAt = DateTime.UtcNow;
    }
}
