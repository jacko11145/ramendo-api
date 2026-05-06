using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ramendo.Domain.Aggregates.Shops;

namespace Ramendo.Infrastructure.Persistence.Configurations;

public sealed class MenuItemOptionValueConfiguration : IEntityTypeConfiguration<MenuItemOptionValue>
{
    public void Configure(EntityTypeBuilder<MenuItemOptionValue> builder)
    {
        builder.ToTable("MenuItemOptionValue");
        builder.HasKey(v => v.Id);
        builder.HasOne(v => v.OptionValue)
            .WithMany()
            .HasForeignKey(v => v.OptionValueId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
