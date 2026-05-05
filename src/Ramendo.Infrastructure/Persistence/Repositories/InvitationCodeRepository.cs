using Microsoft.EntityFrameworkCore;
using Ramendo.Domain.Aggregates.InvitationCodes;

namespace Ramendo.Infrastructure.Persistence.Repositories;

public sealed class InvitationCodeRepository(RamendoDbContext db) : IInvitationCodeRepository
{
    public Task<InvitationCode?> GetByCodeAsync(string code, CancellationToken ct = default) =>
        db.InvitationCodes.FirstOrDefaultAsync(c => c.Code == code, ct);

    public async Task<IReadOnlyList<InvitationCode>> GetAllAsync(CancellationToken ct = default) =>
        await db.InvitationCodes.OrderByDescending(c => c.CreatedAt).ToListAsync(ct);

    public async Task AddAsync(InvitationCode code, CancellationToken ct = default)
    {
        db.InvitationCodes.Add(code); await db.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(InvitationCode code, CancellationToken ct = default)
    {
        db.InvitationCodes.Update(code); await db.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(string code, CancellationToken ct = default)
    {
        var entity = await db.InvitationCodes.FindAsync([code], ct);
        if (entity != null) { db.InvitationCodes.Remove(entity); await db.SaveChangesAsync(ct); }
    }
}
