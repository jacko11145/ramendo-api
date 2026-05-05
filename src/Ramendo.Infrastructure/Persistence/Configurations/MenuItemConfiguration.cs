using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ramendo.Domain.Aggregates.Shops;

namespace Ramendo.Infrastructure.Persistence.Configurations;

public sealed class MenuItemConfiguration : IEntityTypeConfiguration<MenuItem>
{
    public void Configure(EntityTypeBuilder<MenuItem> builder)
    {
        builder.ToTable("MenuItem");
        builder.HasKey(m => m.Id);
        builder.Property(m => m.Name).IsRequired();
        builder.Property(m => m.Price).IsRequired();
        builder.Property(m => m.Category).HasDefaultValue("主食");
        builder.Property(m => m.Status).HasDefaultValue("active");
        builder.Property(m => m.Position).HasDefaultValue(0);
        builder.Property(m => m.IsHighlight).HasDefaultValue(false);
        builder.Property(m => m.IsLimited).HasDefaultValue(false);
        builder.Property(m => m.ShopId).IsRequired();

        builder.HasMany(m => m.ItemOptions)
            .WithOne()
            .HasForeignKey(o => o.MenuItemId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
