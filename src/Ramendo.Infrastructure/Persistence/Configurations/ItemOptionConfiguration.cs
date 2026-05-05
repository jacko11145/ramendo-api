using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ramendo.Domain.Aggregates.Shops;

namespace Ramendo.Infrastructure.Persistence.Configurations;

public sealed class ItemOptionConfiguration : IEntityTypeConfiguration<ItemOption>
{
    public void Configure(EntityTypeBuilder<ItemOption> builder)
    {
        builder.ToTable("ItemOption");
        builder.HasKey(o => o.Id);
        builder.HasMany(o => o.OptionValues)
            .WithOne()
            .HasForeignKey(v => v.ItemOptionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
