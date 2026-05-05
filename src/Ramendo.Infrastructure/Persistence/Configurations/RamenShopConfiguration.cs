using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ramendo.Domain.Aggregates.Shops;

namespace Ramendo.Infrastructure.Persistence.Configurations;

public sealed class RamenShopConfiguration : IEntityTypeConfiguration<RamenShop>
{
    public void Configure(EntityTypeBuilder<RamenShop> builder)
    {
        builder.ToTable("RamenShop");
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Guid).HasColumnName("guid").IsRequired();
        builder.HasIndex(s => s.Guid).IsUnique();
        builder.Property(s => s.Name).IsRequired();
        builder.Property(s => s.City).IsRequired();
        builder.Property(s => s.District).IsRequired();
        builder.Property(s => s.DetailAddress).IsRequired();
        builder.Property(s => s.IsActive).HasDefaultValue(true);
        builder.Property(s => s.IsVerified).HasDefaultValue(false);
        builder.Property(s => s.Rating).HasDefaultValue(0f);
        builder.Property(s => s.GoogleRating).HasDefaultValue(0f);
        builder.Property(s => s.CriticRating).HasDefaultValue(0f);
        builder.Property(s => s.ReviewCount).HasDefaultValue(0);

        builder.Property(s => s.Images)
            .HasColumnType("text[]")
            .HasDefaultValueSql("ARRAY[]::text[]");

        builder.Property(s => s.Types)
            .HasColumnType("text[]")
            .HasDefaultValueSql("ARRAY[]::text[]");

        builder.Property(s => s.BusinessHours)
            .HasColumnType("jsonb")
            .HasConversion(
                v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions?)null),
                v => System.Text.Json.JsonSerializer.Deserialize<BusinessHours>(v, (System.Text.Json.JsonSerializerOptions?)null));

        builder.Property(s => s.NewsItems)
            .HasColumnType("jsonb")
            .HasConversion(
                v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions?)null),
                v => System.Text.Json.JsonSerializer.Deserialize<List<NewsItem>>(v, (System.Text.Json.JsonSerializerOptions?)null) ?? new());

        builder.HasMany(s => s.MenuItems)
            .WithOne()
            .HasForeignKey(m => m.ShopId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
