using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskTracker.Domain.Entities;
using TaskTracker.Domain.Enums;

namespace TaskTracker.Infrastructure.Persistence.Configurations;

public class TaskItemConfiguration : IEntityTypeConfiguration<TaskItem>
{
    public void Configure(EntityTypeBuilder<TaskItem> builder)
    {
        builder.ToTable("tasks");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Title)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(t => t.Description)
            .HasMaxLength(5000);

        builder.Property(t => t.CreatedBy)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(t => t.Status)
            .HasConversion(
                v => v.ToString().ToLower(),
                v => Enum.Parse<TaskItemStatus>(v, true));

        builder.Property(t => t.Priority)
            .HasConversion(
                v => v.ToString().ToLower(),
                v => Enum.Parse<TaskPriority>(v, true));

        builder.HasOne<Project>()
            .WithMany()
            .HasForeignKey(t => t.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<Sprint>()
            .WithMany()
            .HasForeignKey(t => t.SprintId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne<SprintCeremony>()
            .WithMany()
            .HasForeignKey(t => t.CreatedInCeremonyId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(t => t.ProjectId);
        builder.HasIndex(t => t.SprintId);
        builder.HasIndex(t => t.AssignedUserId);
        builder.HasIndex(t => t.AssignedTeamId);
        builder.HasIndex(t => t.CreatedInCeremonyId);
    }
}
