using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskTracker.Domain.Entities;

namespace TaskTracker.Infrastructure.Persistence.Configurations;

public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.ToTable("notifications");

        builder.HasKey(n => n.Id);

        builder.Property(n => n.UserId)
            .IsRequired();

        builder.Property(n => n.Type)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(n => n.Payload)
            .IsRequired()
            .HasColumnType("jsonb");

        builder.HasIndex(n => n.UserId);
        builder.HasIndex(n => n.CreatedAt);
    }
}
