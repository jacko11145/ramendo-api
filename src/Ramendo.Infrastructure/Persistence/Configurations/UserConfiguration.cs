using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ramendo.Domain.Aggregates.Users;

namespace Ramendo.Infrastructure.Persistence.Configurations;

public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("User");
        builder.HasKey(u => u.Id);
        builder.Property(u => u.Id).HasColumnName("id");
        builder.Property(u => u.Email).HasColumnName("email").HasMaxLength(255).IsRequired();
        builder.HasIndex(u => u.Email).IsUnique();
        builder.Property(u => u.Name).HasColumnName("name");
        builder.Property(u => u.Image).HasColumnName("image");
        builder.Property(u => u.PasswordHash).HasColumnName("password");
        builder.Property(u => u.IsActive).HasColumnName("is_active").HasDefaultValue(true);
        builder.Property(u => u.InvitedByCode).HasColumnName("invited_by_code");
        builder.Property(u => u.CreatedAt).HasColumnName("created_at");
        builder.Property(u => u.UpdatedAt).HasColumnName("updated_at");

        builder.Property(u => u.Role)
            .HasColumnName("role")
            .HasConversion(r => r.ToString().ToLower(), s => Enum.Parse<UserRole>(s, true))
            .HasDefaultValue(UserRole.User);

        builder.OwnsOne(u => u.Experience, exp =>
        {
            exp.Property(e => e.Points).HasColumnName("experience_points").HasDefaultValue(0);
            exp.Ignore(e => e.Level);
        });

        builder.OwnsOne(u => u.VIP, vip =>
        {
            vip.Property(v => v.IsVIP).HasColumnName("is_v_i_p").HasDefaultValue(false);
            vip.Property(v => v.MembershipExpiry).HasColumnName("membership_expiry");
            vip.Ignore(v => v.IsActive);
        });
    }
}
