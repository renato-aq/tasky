using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskTracker.Domain.Entities;
using TaskTracker.Domain.Enums;

namespace TaskTracker.Infrastructure.Persistence.Configurations;

public class SprintCeremonyConfiguration : IEntityTypeConfiguration<SprintCeremony>
{
    public void Configure(EntityTypeBuilder<SprintCeremony> builder)
    {
        builder.ToTable("sprint_ceremonies");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Type)
            .HasConversion(
                v => v.ToString().ToLower(),
                v => Enum.Parse<CeremonyType>(v, true));

        builder.Property(c => c.Notes)
            .HasMaxLength(2000);
    }
}
