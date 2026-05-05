using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ramendo.Domain.Aggregates.Submissions;

namespace Ramendo.Infrastructure.Persistence.Configurations;

public sealed class ShopSubmissionConfiguration : IEntityTypeConfiguration<ShopSubmission>
{
    public void Configure(EntityTypeBuilder<ShopSubmission> builder)
    {
        builder.ToTable("ShopSubmission");
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Name).IsRequired();
        builder.Property(s => s.City).IsRequired();
        builder.Property(s => s.District).IsRequired();
        builder.Property(s => s.DetailAddress).IsRequired();
        builder.Property(s => s.Status)
            .HasConversion(s => s.ToString().ToLower(), s => Enum.Parse<SubmissionStatus>(s, true))
            .HasDefaultValue(SubmissionStatus.Pending);
        builder.Property(s => s.Images)
            .HasColumnType("text[]")
            .HasDefaultValueSql("ARRAY[]::text[]");
        builder.Property(s => s.Types)
            .HasColumnType("text[]")
            .HasDefaultValueSql("ARRAY[]::text[]");
    }
}
