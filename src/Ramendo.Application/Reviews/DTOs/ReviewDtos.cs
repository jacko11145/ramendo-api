namespace Ramendo.Application.Reviews.DTOs;

public sealed record ReviewDto(
    string Id, float Rating, string Content, string[] Images,
    string UserId, string? UserName, string? UserImage,
    string ShopId, DateTime CreatedAt);

public sealed record CreateReviewDto(Guid ShopGuid, float Rating, string Content, string[] Images);

public sealed record UpdateReviewDto(float Rating, string Content, string[] Images);

public sealed record MenuItemRatingDto(
    string Id, float Rating, string? Comment, string UserId, string MenuItemId,
    string[] SelectedOptionValueIds, DateTime CreatedAt);

public sealed record CreateMenuItemRatingDto(float Rating, string? Comment, string[] SelectedOptionValueIds);
