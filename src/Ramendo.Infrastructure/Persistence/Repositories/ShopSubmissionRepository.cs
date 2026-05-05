using Microsoft.EntityFrameworkCore;
using Ramendo.Domain.Aggregates.Submissions;

namespace Ramendo.Infrastructure.Persistence.Repositories;

public sealed class ShopSubmissionRepository(RamendoDbContext db) : IShopSubmissionRepository
{
    public Task<ShopSubmission?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        db.ShopSubmissions.FirstOrDefaultAsync(s => s.Id == id, ct);

    public async Task<(IReadOnlyList<ShopSubmission> Items, int Total)> GetAllAsync(int page, int limit, CancellationToken ct = default)
    {
        var q = db.ShopSubmissions.OrderByDescending(s => s.CreatedAt);
        var total = await q.CountAsync(ct);
        var items = await q.Skip((page - 1) * limit).Take(limit).ToListAsync(ct);
        return (items, total);
    }

    public async Task<IReadOnlyList<ShopSubmission>> GetByUserAsync(Guid userId, CancellationToken ct = default) =>
        await db.ShopSubmissions.Where(s => s.UserId == userId).OrderByDescending(s => s.CreatedAt).ToListAsync(ct);

    public Task<int> CountPendingAsync(CancellationToken ct = default) =>
        db.ShopSubmissions.CountAsync(s => s.Status == SubmissionStatus.Pending, ct);

    public async Task AddAsync(ShopSubmission submission, CancellationToken ct = default)
    {
        db.ShopSubmissions.Add(submission); await db.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(ShopSubmission submission, CancellationToken ct = default)
    {
        db.ShopSubmissions.Update(submission); await db.SaveChangesAsync(ct);
    }
}
