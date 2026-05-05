namespace Ramendo.Application.Settings.DTOs;

public sealed record PermissionSettingsDto(
    int MinLevelToSubmitShop, int MinLevelToWriteReview, int MinLevelToRateMenuItem);

public sealed record DashboardStatsDto(
    int TotalUsers, int TotalShops, int TotalReviews,
    int PendingSubmissions, int ActiveInvitationCodes,
    int NewUsersThisMonth, int NewReviewsThisMonth);

public sealed record TableStatsDto(string TableName, int RowCount, int SizeKb);
