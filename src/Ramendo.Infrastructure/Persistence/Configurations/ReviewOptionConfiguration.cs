using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ramendo.Domain.Aggregates.Reviews;

namespace Ramendo.Infrastructure.Persistence.Configurations;

public sealed class ReviewOptionConfiguration : IEntityTypeConfiguration<ReviewOption>
{
    public void Configure(EntityTypeBuilder<ReviewOption> builder)
    {
        builder.ToTable("ReviewOption");
        builder.HasKey(o => o.Id);
    }
}
