using Microsoft.EntityFrameworkCore;
using Ramendo.Domain.Aggregates.Shops;

namespace Ramendo.Infrastructure.Persistence.Repositories;

public sealed class RamenShopRepository(RamendoDbContext db) : IRamenShopRepository
{
    public Task<RamenShop?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        db.RamenShops
            .Include(s => s.MenuItems)
                .ThenInclude(m => m.ItemOptions)
                    .ThenInclude(io => io.OptionType)
            .Include(s => s.MenuItems)
                .ThenInclude(m => m.ItemOptions)
                    .ThenInclude(io => io.OptionValues)
                        .ThenInclude(ov => ov.OptionValue)
            .FirstOrDefaultAsync(s => s.Id == id, ct);

    public Task<RamenShop?> GetByGuidAsync(Guid guid, CancellationToken ct = default) =>
        db.RamenShops
            .Include(s => s.MenuItems)
                .ThenInclude(m => m.ItemOptions)
                    .ThenInclude(io => io.OptionType)
            .Include(s => s.MenuItems)
                .ThenInclude(m => m.ItemOptions)
                    .ThenInclude(io => io.OptionValues)
                        .ThenInclude(ov => ov.OptionValue)
            .FirstOrDefaultAsync(s => s.Guid == guid, ct);

    public async Task<(IReadOnlyList<RamenShop> Items, int Total)> GetPagedAsync(
        string? city, string? district, string[]? types, string sort, int page, int limit,
        string? search = null, bool adminMode = false, CancellationToken ct = default)
    {
        var q = db.RamenShops.AsQueryable();
        if (!adminMode) q = q.Where(s => s.IsActive);
        if (!string.IsNullOrEmpty(search)) q = q.Where(s => s.Name.Contains(search));
        if (!string.IsNullOrEmpty(city)) q = q.Where(s => s.City == city);
        if (!string.IsNullOrEmpty(district)) q = q.Where(s => s.District == district);
        if (types?.Length > 0) q = q.Where(s => s.Types.Any(t => types.Contains(t)));

        q = sort == "newest" ? q.OrderByDescending(s => s.CreatedAt) : q.OrderByDescending(s => s.Rating);

        var total = await q.CountAsync(ct);
        var items = await q.Skip((page - 1) * limit).Take(limit).ToListAsync(ct);
        return (items, total);
    }

    public async Task<IReadOnlyList<RamenShop>> GetForRankingsAsync(CancellationToken ct = default) =>
        await db.RamenShops.Where(s => s.IsActive).ToListAsync(ct);

    public Task<int> CountAsync(CancellationToken ct = default) => db.RamenShops.CountAsync(ct);

    public async Task AddAsync(RamenShop shop, CancellationToken ct = default)
    {
        db.RamenShops.Add(shop);
        await db.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(RamenShop shop, CancellationToken ct = default)
    {
        db.RamenShops.Update(shop);
        await db.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var shop = await db.RamenShops.FindAsync([id], ct);
        if (shop != null) { db.RamenShops.Remove(shop); await db.SaveChangesAsync(ct); }
    }
}
