namespace Ramendo.Application.Favorites.DTOs;

public sealed record FavoriteShopDto(
    string ShopId, string ShopGuid, string Name, string City, string District,
    string? CoverImage, float Rating, string[] Types, DateTime CreatedAt);
