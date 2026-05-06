using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ramendo.Infrastructure.Persistence.Configurations;

public sealed class SystemSettingConfiguration : IEntityTypeConfiguration<SystemSetting>
{
    public void Configure(EntityTypeBuilder<SystemSetting> builder)
    {
        builder.ToTable("SystemSetting");
        builder.HasKey(s => s.Key);
        builder.Property(s => s.Value).HasColumnType("jsonb").HasDefaultValue("{}");
    }
}
