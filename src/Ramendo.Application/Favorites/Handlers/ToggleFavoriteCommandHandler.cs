using MediatR;
using Ramendo.Application.Common;
using Ramendo.Application.Favorites.Commands;
using Ramendo.Domain.Aggregates.Shops;
using Ramendo.Domain.Aggregates.Users;
using Ramendo.Domain.Services;

namespace Ramendo.Application.Favorites.Handlers;

public sealed class ToggleFavoriteCommandHandler(
    IFavoriteRepository favorites,
    IRamenShopRepository shops,
    IUserRepository users,
    IPermissionService permissions) : IRequestHandler<ToggleFavoriteCommand, bool>
{
    public async Task<bool> Handle(ToggleFavoriteCommand cmd, CancellationToken ct)
    {
        var shop = await shops.GetByGuidAsync(cmd.ShopGuid, ct)
            ?? throw new NotFoundException("RamenShop", cmd.ShopGuid);

        var user = await users.GetByIdAsync(cmd.UserId, ct)
            ?? throw new NotFoundException("User", cmd.UserId);

        if (user.Role == UserRole.User && !await permissions.CanFavoriteAsync(user.Experience.Level, ct))
            throw new ForbiddenException("您的等級不足以加入收藏。");

        var existing = await favorites.GetAsync(cmd.UserId, shop.Id, ct);

        if (existing is not null)
        {
            await favorites.DeleteAsync(cmd.UserId, shop.Id, ct);
            return false; // unfavorited
        }

        await favorites.AddAsync(Favorite.Create(cmd.UserId, shop.Id), ct);
        return true; // favorited
    }
}
