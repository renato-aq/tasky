using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskTracker.Domain.Entities;
using TaskTracker.Domain.Enums;

namespace TaskTracker.Infrastructure.Persistence.Configurations;

public class SubTaskItemConfiguration : IEntityTypeConfiguration<SubTaskItem>
{
    public void Configure(EntityTypeBuilder<SubTaskItem> builder)
    {
        builder.ToTable("sub_tasks");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Title)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(s => s.Status)
            .HasConversion(
                v => v.ToString().ToLower(),
                v => Enum.Parse<SubTaskStatus>(v, true));

        builder.HasOne<TaskItem>()
            .WithMany()
            .HasForeignKey(s => s.TaskId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(s => s.TaskId);
        builder.HasIndex(s => s.AssignedUserId);
    }
}
