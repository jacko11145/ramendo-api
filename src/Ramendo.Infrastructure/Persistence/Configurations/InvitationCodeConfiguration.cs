using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ramendo.Domain.Aggregates.InvitationCodes;

namespace Ramendo.Infrastructure.Persistence.Configurations;

public sealed class InvitationCodeConfiguration : IEntityTypeConfiguration<InvitationCode>
{
    public void Configure(EntityTypeBuilder<InvitationCode> builder)
    {
        builder.ToTable("InvitationCode");
        builder.HasKey(c => c.Code);
        builder.Property(c => c.Code).HasMaxLength(50);
        builder.Property(c => c.IsActive).HasDefaultValue(true);
        builder.Property(c => c.MaxUses).HasDefaultValue(1);
        builder.Property(c => c.UsedCount).HasDefaultValue(0);
    }
}
