using Microsoft.EntityFrameworkCore;
using Ramendo.Application.Common;

namespace Ramendo.Infrastructure.Persistence.Repositories;

public sealed class SystemSettingsRepository(RamendoDbContext db) : ISystemSettingsRepository
{
    public async Task<string?> GetAsync(string key, CancellationToken ct = default)
    {
        var row = await db.SystemSettings.AsNoTracking().FirstOrDefaultAsync(s => s.Key == key, ct);
        return row?.Value;
    }

    public async Task SetAsync(string key, string jsonValue, CancellationToken ct = default)
    {
        var row = await db.SystemSettings.FirstOrDefaultAsync(s => s.Key == key, ct);
        if (row is null)
        {
            db.SystemSettings.Add(new SystemSetting { Key = key, Value = jsonValue, UpdatedAt = DateTime.UtcNow });
        }
        else
        {
            row.Value = jsonValue;
            row.UpdatedAt = DateTime.UtcNow;
        }
        await db.SaveChangesAsync(ct);
    }
}
