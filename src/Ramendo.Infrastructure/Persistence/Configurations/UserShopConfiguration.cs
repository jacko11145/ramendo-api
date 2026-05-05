using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ramendo.Domain.Aggregates.Users;

namespace Ramendo.Infrastructure.Persistence.Configurations;

public sealed class UserShopConfiguration : IEntityTypeConfiguration<UserShop>
{
    public void Configure(EntityTypeBuilder<UserShop> builder)
    {
        builder.ToTable("UserShop");
        builder.HasKey(u => u.Id);
        builder.HasIndex(u => new { u.UserId, u.ShopId }).IsUnique();
        builder.Property(u => u.Role).HasDefaultValue("owner");
    }
}
