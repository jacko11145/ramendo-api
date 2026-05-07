namespace Ramendo.Application.Reviews.DTOs;

public sealed record ReviewDto(
    string Id, float Rating, string? Comment, int UserLevel,
    string? VisitDate, string UserId, string? UserName, string? UserImage,
    string ShopId, DateTime CreatedAt, string[] Images);

public sealed record CreateReviewDto(
    Guid ShopGuid, float Rating, string? Comment,
    DateOnly? VisitDate = null, string[]? Images = null);

public sealed record UpdateReviewDto(float Rating, string? Comment, string[]? Images = null);

public sealed record AdminReviewDto(
    string Id, float Rating, string? Comment, string? VisitDate,
    string UserId, string? UserName, string ShopId, string? ShopName,
    DateTime CreatedAt);

public sealed record MenuItemRatingDto(
    string Id, float Rating, string? Comment, string UserId, string MenuItemId,
    string[] SelectedOptionValueIds, DateTime CreatedAt);

public sealed record CreateMenuItemRatingDto(float Rating, string? Comment, string[] SelectedOptionValueIds);
