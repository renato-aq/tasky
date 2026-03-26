using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskTracker.Domain.Entities;
using TaskTracker.Domain.Enums;

namespace TaskTracker.Infrastructure.Persistence.Configurations;

public class SprintConfiguration : IEntityTypeConfiguration<Sprint>
{
    public void Configure(EntityTypeBuilder<Sprint> builder)
    {
        builder.ToTable("sprints");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(s => s.Goal)
            .HasMaxLength(500);

        builder.Property(s => s.Status)
            .HasConversion(
                v => v.ToString().ToLower(),
                v => Enum.Parse<SprintStatus>(v, true));

        builder.HasOne<Project>()
            .WithMany()
            .HasForeignKey(s => s.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(s => s.Ceremonies)
            .WithOne(c => c.Sprint)
            .HasForeignKey(c => c.SprintId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
