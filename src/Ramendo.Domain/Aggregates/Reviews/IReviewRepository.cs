namespace Ramendo.Domain.Aggregates.Reviews;

public interface IReviewRepository
{
    Task<Review?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<(IReadOnlyList<Review> Items, int Total)> GetByShopAsync(Guid shopId, int page, int limit, CancellationToken ct = default);
    Task<IReadOnlyList<Review>> GetByUserAsync(Guid userId, CancellationToken ct = default);
    Task<(IReadOnlyList<Review> Items, int Total)> GetAllAsync(int page, int limit, CancellationToken ct = default);
    Task<int> CountAsync(CancellationToken ct = default);
    Task<int> CountSinceAsync(DateTime since, CancellationToken ct = default);
    Task<Dictionary<Guid, int>> GetCountsByUsersAsync(IEnumerable<Guid> userIds, CancellationToken ct = default);
    Task<(float AvgRating, int Count)> GetRatingStatsAsync(Guid shopId, CancellationToken ct = default);
    Task AddAsync(Review review, CancellationToken ct = default);
    Task UpdateAsync(Review review, CancellationToken ct = default);
    Task DeleteAsync(Guid id, CancellationToken ct = default);
    Task<MenuItemRating?> GetMenuItemRatingAsync(Guid userId, Guid menuItemId, CancellationToken ct = default);
    Task AddMenuItemRatingAsync(MenuItemRating rating, CancellationToken ct = default);
    Task UpdateMenuItemRatingAsync(MenuItemRating rating, CancellationToken ct = default);
    Task DeleteMenuItemRatingAsync(Guid userId, Guid menuItemId, CancellationToken ct = default);
}
