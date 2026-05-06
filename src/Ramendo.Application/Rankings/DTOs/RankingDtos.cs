namespace Ramendo.Application.Rankings.DTOs;

public sealed record RankingItemDto(
    int Rank, string ShopGuid, string ShopName, string City, string District,
    string? CoverImage, float Score, float UserRating, float GoogleRating,
    int ReviewCount, string[] Types);

public sealed record RankingSettingsDto(
    bool AllowUser, bool AllowGoogle, bool AllowCombined,
    string DefaultType, float UserWeight, float GoogleWeight,
    int DisplayLimit, int MinReviews, float MinRating, bool MustBeVerified);
