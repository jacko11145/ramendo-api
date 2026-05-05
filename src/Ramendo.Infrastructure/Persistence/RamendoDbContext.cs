using Microsoft.EntityFrameworkCore;
using Ramendo.Domain.Aggregates.InvitationCodes;
using Ramendo.Domain.Aggregates.Reviews;
using Ramendo.Domain.Aggregates.Shops;
using Ramendo.Domain.Aggregates.Submissions;
using Ramendo.Domain.Aggregates.Users;

namespace Ramendo.Infrastructure.Persistence;

public sealed class RamendoDbContext(DbContextOptions<RamendoDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<RamenShop> RamenShops => Set<RamenShop>();
    public DbSet<MenuItem> MenuItems => Set<MenuItem>();
    public DbSet<OptionType> OptionTypes => Set<OptionType>();
    public DbSet<OptionValue> OptionValues => Set<OptionValue>();
    public DbSet<ItemOption> ItemOptions => Set<ItemOption>();
    public DbSet<MenuItemOptionValue> MenuItemOptionValues => Set<MenuItemOptionValue>();
    public DbSet<Review> Reviews => Set<Review>();
    public DbSet<MenuItemRating> MenuItemRatings => Set<MenuItemRating>();
    public DbSet<ReviewOption> ReviewOptions => Set<ReviewOption>();
    public DbSet<ShopSubmission> ShopSubmissions => Set<ShopSubmission>();
    public DbSet<InvitationCode> InvitationCodes => Set<InvitationCode>();
    public DbSet<Favorite> Favorites => Set<Favorite>();
    public DbSet<UserShop> UserShops => Set<UserShop>();
    public DbSet<SystemSetting> SystemSettings => Set<SystemSetting>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(RamendoDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
