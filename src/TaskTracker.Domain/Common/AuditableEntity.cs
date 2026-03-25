namespace TaskTracker.Domain.Common;

public abstract class AuditableEntity<TId> : Entity<TId>
{
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    protected AuditableEntity() { }

    protected AuditableEntity(TId id) : base(id)
    {
        CreatedAt = DateTime.UtcNow;
    }
}
