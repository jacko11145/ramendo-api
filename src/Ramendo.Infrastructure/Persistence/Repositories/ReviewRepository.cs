using Microsoft.EntityFrameworkCore;
using Ramendo.Domain.Aggregates.Reviews;

namespace Ramendo.Infrastructure.Persistence.Repositories;

public sealed class ReviewRepository(RamendoDbContext db) : IReviewRepository
{
    public Task<Review?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        db.Reviews.FirstOrDefaultAsync(r => r.Id == id, ct);

    public async Task<(IReadOnlyList<Review> Items, int Total)> GetByShopAsync(
        Guid shopId, int page, int limit, CancellationToken ct = default)
    {
        var q = db.Reviews.Include(r => r.User)
            .Where(r => r.RamenShopId == shopId).OrderByDescending(r => r.CreatedAt);
        var total = await q.CountAsync(ct);
        var items = await q.Skip((page - 1) * limit).Take(limit).ToListAsync(ct);
        return (items, total);
    }

    public async Task<IReadOnlyList<Review>> GetByUserAsync(Guid userId, CancellationToken ct = default) =>
        await db.Reviews.Where(r => r.UserId == userId).OrderByDescending(r => r.CreatedAt).ToListAsync(ct);

    public async Task<(IReadOnlyList<Review> Items, int Total)> GetAllAsync(int page, int limit, CancellationToken ct = default)
    {
        var q = db.Reviews.Include(r => r.User).OrderByDescending(r => r.CreatedAt);
        var total = await q.CountAsync(ct);
        var items = await q.Skip((page - 1) * limit).Take(limit).ToListAsync(ct);
        return (items, total);
    }

    public Task<int> CountAsync(CancellationToken ct = default) => db.Reviews.CountAsync(ct);

    public Task<int> CountSinceAsync(DateTime since, CancellationToken ct = default) =>
        db.Reviews.CountAsync(r => r.CreatedAt >= since, ct);

    public async Task<Dictionary<Guid, int>> GetCountsByUsersAsync(IEnumerable<Guid> userIds, CancellationToken ct = default)
    {
        var idList = userIds.ToList();
        return await db.Reviews
            .Where(r => idList.Contains(r.UserId))
            .GroupBy(r => r.UserId)
            .Select(g => new { UserId = g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.UserId, x => x.Count, ct);
    }

    public async Task<(float AvgRating, int Count)> GetRatingStatsAsync(Guid shopId, CancellationToken ct = default)
    {
        var reviews = await db.Reviews.Where(r => r.RamenShopId == shopId).Select(r => r.Rating).ToListAsync(ct);
        return reviews.Count == 0 ? (0f, 0) : ((float)reviews.Average(), reviews.Count);
    }

    public async Task AddAsync(Review review, CancellationToken ct = default)
    {
        db.Reviews.Add(review); await db.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(Review review, CancellationToken ct = default)
    {
        db.Reviews.Update(review); await db.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var review = await db.Reviews.FindAsync([id], ct);
        if (review != null) { db.Reviews.Remove(review); await db.SaveChangesAsync(ct); }
    }

    public Task<MenuItemRating?> GetMenuItemRatingAsync(Guid userId, Guid menuItemId, CancellationToken ct = default) =>
        db.MenuItemRatings.Include(r => r.ReviewOptions)
            .FirstOrDefaultAsync(r => r.UserId == userId && r.MenuItemId == menuItemId, ct);

    public async Task AddMenuItemRatingAsync(MenuItemRating rating, CancellationToken ct = default)
    {
        db.MenuItemRatings.Add(rating); await db.SaveChangesAsync(ct);
    }

    public async Task UpdateMenuItemRatingAsync(MenuItemRating rating, CancellationToken ct = default)
    {
        db.MenuItemRatings.Update(rating); await db.SaveChangesAsync(ct);
    }

    public async Task DeleteMenuItemRatingAsync(Guid userId, Guid menuItemId, CancellationToken ct = default)
    {
        var rating = await db.MenuItemRatings
            .FirstOrDefaultAsync(r => r.UserId == userId && r.MenuItemId == menuItemId, ct);
        if (rating != null) { db.MenuItemRatings.Remove(rating); await db.SaveChangesAsync(ct); }
    }
}
