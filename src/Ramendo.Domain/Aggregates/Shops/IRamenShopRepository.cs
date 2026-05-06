namespace Ramendo.Domain.Aggregates.Shops;

public interface IRamenShopRepository
{
    Task<RamenShop?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<RamenShop?> GetByGuidAsync(Guid guid, CancellationToken ct = default);
    Task<(IReadOnlyList<RamenShop> Items, int Total)> GetPagedAsync(
        string? city, string? district, string[]? types, string sort, int page, int limit,
        string? search = null, bool adminMode = false, CancellationToken ct = default);
    Task<IReadOnlyList<RamenShop>> GetForRankingsAsync(CancellationToken ct = default);
    Task<IReadOnlyList<RamenShop>> GetManyByIdsAsync(IEnumerable<Guid> ids, CancellationToken ct = default);
    Task<int> CountAsync(CancellationToken ct = default);
    Task AddAsync(RamenShop shop, CancellationToken ct = default);
    Task UpdateAsync(RamenShop shop, CancellationToken ct = default);
    Task DeleteAsync(Guid id, CancellationToken ct = default);
}
