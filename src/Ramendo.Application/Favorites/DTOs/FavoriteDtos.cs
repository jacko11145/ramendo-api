namespace Ramendo.Application.Favorites.DTOs;

public sealed record FavoriteShopDto(
    string ShopGuid, string ShopName, string City, string District,
    string? CoverImage, bool IsOpen, DateTime FavoritedAt);
