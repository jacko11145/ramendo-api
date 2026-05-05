using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ramendo.Domain.Aggregates.Reviews;

namespace Ramendo.Infrastructure.Persistence.Configurations;

public sealed class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.ToTable("Review");
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Content).IsRequired();
        builder.Property(r => r.Rating).IsRequired();
        builder.Property(r => r.UserId).IsRequired();
        builder.Property(r => r.RamenShopId).IsRequired();
        builder.Property(r => r.Images)
            .HasColumnType("text[]")
            .HasDefaultValueSql("ARRAY[]::text[]");
    }
}
