using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Ramendo.Domain.Services;
using Ramendo.Infrastructure.Persistence;

namespace Ramendo.Infrastructure.Services;

public sealed record PermissionSettings(int MinLevelToSubmitShop, int MinLevelToReview, int MinLevelToFavorite);

public sealed class PermissionService(RamendoDbContext db) : IPermissionService
{
    private static readonly PermissionSettings Defaults = new(1, 1, 1);

    public async Task<bool> CanSubmitShopAsync(int userLevel, CancellationToken ct = default) =>
        userLevel >= (await GetSettingsAsync(ct)).MinLevelToSubmitShop;

    public async Task<bool> CanReviewAsync(int userLevel, CancellationToken ct = default) =>
        userLevel >= (await GetSettingsAsync(ct)).MinLevelToReview;

    public async Task<bool> CanFavoriteAsync(int userLevel, CancellationToken ct = default) =>
        userLevel >= (await GetSettingsAsync(ct)).MinLevelToFavorite;

    private async Task<PermissionSettings> GetSettingsAsync(CancellationToken ct)
    {
        var row = await db.SystemSettings.AsNoTracking().FirstOrDefaultAsync(s => s.Key == "permission_settings", ct);
        if (row == null) return Defaults;
        try { return JsonSerializer.Deserialize<PermissionSettings>(row.Value) ?? Defaults; }
        catch { return Defaults; }
    }
}
