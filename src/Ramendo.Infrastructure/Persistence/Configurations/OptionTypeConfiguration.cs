using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ramendo.Domain.Aggregates.Shops;

namespace Ramendo.Infrastructure.Persistence.Configurations;

public sealed class OptionTypeConfiguration : IEntityTypeConfiguration<OptionType>
{
    public void Configure(EntityTypeBuilder<OptionType> builder)
    {
        builder.ToTable("OptionType");
        builder.HasKey(o => o.Id);
        builder.Property(o => o.Name).IsRequired();
        builder.HasMany(o => o.OptionValues)
            .WithOne()
            .HasForeignKey(v => v.OptionTypeId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
