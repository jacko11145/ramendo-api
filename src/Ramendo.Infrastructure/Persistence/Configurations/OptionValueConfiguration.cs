using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ramendo.Domain.Aggregates.Shops;

namespace Ramendo.Infrastructure.Persistence.Configurations;

public sealed class OptionValueConfiguration : IEntityTypeConfiguration<OptionValue>
{
    public void Configure(EntityTypeBuilder<OptionValue> builder)
    {
        builder.ToTable("OptionValue");
        builder.HasKey(v => v.Id);
        builder.Property(v => v.Value).IsRequired();
    }
}
