using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Ramendo.Domain.Services;
using Ramendo.Infrastructure.Persistence;

namespace Ramendo.Infrastructure.Services;

public sealed record PermissionSettings(int MinLevelToSubmitShop, int MinLevelToWriteReview, int MinLevelToRateMenuItem);

public sealed class PermissionService(RamendoDbContext db) : IPermissionService
{
    private static readonly PermissionSettings Defaults = new(1, 1, 1);

    public async Task<bool> CanSubmitShopAsync(int userLevel, CancellationToken ct = default) =>
        userLevel >= (await GetSettingsAsync(ct)).MinLevelToSubmitShop;

    public async Task<bool> CanWriteReviewAsync(int userLevel, CancellationToken ct = default) =>
        userLevel >= (await GetSettingsAsync(ct)).MinLevelToWriteReview;

    public async Task<bool> CanRateMenuItemAsync(int userLevel, CancellationToken ct = default) =>
        userLevel >= (await GetSettingsAsync(ct)).MinLevelToRateMenuItem;

    private async Task<PermissionSettings> GetSettingsAsync(CancellationToken ct)
    {
        var row = await db.SystemSettings.AsNoTracking().FirstOrDefaultAsync(s => s.Key == "permission_settings", ct);
        if (row == null) return Defaults;
        try { return JsonSerializer.Deserialize<PermissionSettings>(row.Value) ?? Defaults; }
        catch { return Defaults; }
    }
}
