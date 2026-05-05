using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ramendo.Domain.Aggregates.Reviews;

namespace Ramendo.Infrastructure.Persistence.Configurations;

public sealed class MenuItemRatingConfiguration : IEntityTypeConfiguration<MenuItemRating>
{
    public void Configure(EntityTypeBuilder<MenuItemRating> builder)
    {
        builder.ToTable("MenuItemRating");
        builder.HasKey(r => r.Id);
        builder.HasIndex(r => new { r.UserId, r.MenuItemId }).IsUnique();

        builder.HasMany(r => r.ReviewOptions)
            .WithOne()
            .HasForeignKey(o => o.RatingId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
