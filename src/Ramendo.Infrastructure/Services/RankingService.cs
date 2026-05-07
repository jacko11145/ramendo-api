using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Ramendo.Domain.Aggregates.Shops;
using Ramendo.Domain.Services;
using Ramendo.Infrastructure.Persistence;

namespace Ramendo.Infrastructure.Services;

public sealed record RankingSettings(
    bool AllowUser, bool AllowGoogle, bool AllowCombined, string DefaultType,
    float UserWeight, float GoogleWeight, int DisplayLimit,
    int MinReviews, float MinRating, bool MustBeVerified);

public sealed class RankingService(RamendoDbContext db) : IRankingService
{
    private static readonly RankingSettings Defaults = new(
        true, true, true, "user", 1.0f, 0.8f, 20, 0, 0f, false);

    public IReadOnlyList<(RamenShop Shop, float Score)> Rank(IEnumerable<RamenShop> shops, string type)
    {
        var settings = GetSettingsFromDb();

        return shops
            .Where(s => s.IsActive && IsEligible(s, settings))
            .Select(s => (s, ComputeScore(s, type, settings)))
            .OrderByDescending(x => x.Item2)
            .Take(settings.DisplayLimit)
            .ToList();
    }

    private static bool IsEligible(RamenShop s, RankingSettings cfg) =>
        s.ReviewCount >= cfg.MinReviews &&
        s.Rating >= cfg.MinRating &&
        (!cfg.MustBeVerified || s.IsVerified);

    private static float ComputeScore(RamenShop s, string type, RankingSettings cfg) => type switch
    {
        "google" => s.GoogleRating,
        "combined" => CalculateCombined(s, cfg),
        _ => s.Rating
    };

    private static float CalculateCombined(RamenShop s, RankingSettings cfg)
    {
        float sum = 0, weight = 0;
        if (s.Rating > 0) { sum += s.Rating * cfg.UserWeight; weight += cfg.UserWeight; }
        if (s.GoogleRating > 0) { sum += s.GoogleRating * cfg.GoogleWeight; weight += cfg.GoogleWeight; }
        return weight > 0 ? sum / weight : 0;
    }

    private RankingSettings GetSettingsFromDb()
    {
        var row = db.SystemSettings.AsNoTracking().FirstOrDefault(s => s.Key == "ranking_settings");
        if (row == null) return Defaults;
        try { return JsonSerializer.Deserialize<RankingSettings>(row.Value) ?? Defaults; }
        catch { return Defaults; }
    }
}
