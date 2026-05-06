using MediatR;
using Ramendo.Application.Favorites.DTOs;
using Ramendo.Application.Favorites.Queries;
using Ramendo.Domain.Aggregates.Shops;
using Ramendo.Domain.Aggregates.Users;

namespace Ramendo.Application.Favorites.Handlers;

public sealed class GetUserFavoritesQueryHandler(
    IFavoriteRepository favorites,
    IRamenShopRepository shops) : IRequestHandler<GetUserFavoritesQuery, IReadOnlyList<FavoriteShopDto>>
{
    public async Task<IReadOnlyList<FavoriteShopDto>> Handle(GetUserFavoritesQuery q, CancellationToken ct)
    {
        var favs = await favorites.GetByUserAsync(q.UserId, ct);

        var result = new List<FavoriteShopDto>();
        foreach (var fav in favs)
        {
            var shop = await shops.GetByIdAsync(fav.RamenShopId, ct);
            if (shop is not null)
            {
                result.Add(new FavoriteShopDto(
                    shop.Guid.ToString(), shop.Name, shop.City, shop.District,
                    shop.CoverImage, BusinessHoursService.IsOpenNow(shop.BusinessHours),
                    fav.CreatedAt));
            }
        }
        return result;
    }
}
