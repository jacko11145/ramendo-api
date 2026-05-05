using Microsoft.EntityFrameworkCore;
using Ramendo.Domain.Aggregates.Users;

namespace Ramendo.Infrastructure.Persistence.Repositories;

public sealed class FavoriteRepository(RamendoDbContext db) : IFavoriteRepository
{
    public Task<Favorite?> GetAsync(Guid userId, Guid shopId, CancellationToken ct = default) =>
        db.Favorites.FirstOrDefaultAsync(f => f.UserId == userId && f.RamenShopId == shopId, ct);

    public async Task<IReadOnlyList<Favorite>> GetByUserAsync(Guid userId, CancellationToken ct = default) =>
        await db.Favorites.Where(f => f.UserId == userId).OrderByDescending(f => f.CreatedAt).ToListAsync(ct);

    public async Task AddAsync(Favorite favorite, CancellationToken ct = default)
    {
        db.Favorites.Add(favorite);
        await db.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Guid userId, Guid shopId, CancellationToken ct = default)
    {
        var fav = await db.Favorites.FirstOrDefaultAsync(f => f.UserId == userId && f.RamenShopId == shopId, ct);
        if (fav != null) { db.Favorites.Remove(fav); await db.SaveChangesAsync(ct); }
    }
}
