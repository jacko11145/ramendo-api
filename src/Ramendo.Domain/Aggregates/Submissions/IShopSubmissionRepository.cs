namespace Ramendo.Domain.Aggregates.Submissions;

public interface IShopSubmissionRepository
{
    Task<ShopSubmission?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<(IReadOnlyList<ShopSubmission> Items, int Total)> GetAllAsync(int page, int limit, CancellationToken ct = default);
    Task<IReadOnlyList<ShopSubmission>> GetByUserAsync(Guid userId, CancellationToken ct = default);
    Task<int> CountPendingAsync(CancellationToken ct = default);
    Task AddAsync(ShopSubmission submission, CancellationToken ct = default);
    Task UpdateAsync(ShopSubmission submission, CancellationToken ct = default);
}
