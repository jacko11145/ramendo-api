namespace Ramendo.Application.Settings.DTOs;

public sealed record PermissionSettingsDto(
    int MinLevelToSubmitShop, int MinLevelToWriteReview, int MinLevelToRateMenuItem);

public sealed record DashboardStatsDto(
    int TotalUsers, int TotalShops, int TotalMenuItems, int TotalReviews, int TotalFavorites);
