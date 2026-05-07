using Microsoft.EntityFrameworkCore;
using Ramendo.Domain.Aggregates.Users;

namespace Ramendo.Infrastructure.Persistence.Repositories;

public sealed class UserRepository(RamendoDbContext db) : IUserRepository
{
    public Task<User?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        db.Users.FirstOrDefaultAsync(u => u.Id == id, ct);

    public async Task<IReadOnlyList<User>> GetManyByIdsAsync(IEnumerable<Guid> ids, CancellationToken ct = default)
    {
        var idList = ids.ToList();
        return await db.Users.Where(u => idList.Contains(u.Id)).ToListAsync(ct);
    }

    public Task<User?> GetByEmailAsync(string email, CancellationToken ct = default) =>
        db.Users.FirstOrDefaultAsync(u => u.Email == email, ct);

    public async Task<(IReadOnlyList<User> Items, int Total)> GetPagedAsync(
        int page, int limit, string? search = null, string? role = null, CancellationToken ct = default)
    {
        var q = db.Users.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
            q = q.Where(u => u.Email.Contains(search) || (u.Name != null && u.Name.Contains(search)));

        if (!string.IsNullOrWhiteSpace(role) && Enum.TryParse<UserRole>(role, ignoreCase: true, out var parsedRole))
            q = q.Where(u => u.Role == parsedRole);

        var total = await q.CountAsync(ct);
        var items = await q.OrderByDescending(u => u.CreatedAt)
            .Skip((page - 1) * limit).Take(limit).ToListAsync(ct);

        return (items, total);
    }

    public Task<int> CountAsync(CancellationToken ct = default) => db.Users.CountAsync(ct);

    public Task<int> CountSinceAsync(DateTime since, CancellationToken ct = default) =>
        db.Users.CountAsync(u => u.CreatedAt >= since, ct);

    public async Task AddAsync(User user, CancellationToken ct = default)
    {
        db.Users.Add(user);
        await db.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(User user, CancellationToken ct = default)
    {
        db.Users.Update(user);
        await db.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var user = await db.Users.FindAsync([id], ct);
        if (user != null) { db.Users.Remove(user); await db.SaveChangesAsync(ct); }
    }
}
