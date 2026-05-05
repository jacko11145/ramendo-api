using MediatR;
using Ramendo.Application.Common;
using Ramendo.Application.Shops.Commands;
using Ramendo.Domain.Aggregates.Shops;

namespace Ramendo.Application.Shops.Handlers;

public sealed class DeleteMenuItemCommandHandler(IRamenShopRepository shops) : IRequestHandler<DeleteMenuItemCommand>
{
    public async Task Handle(DeleteMenuItemCommand cmd, CancellationToken ct)
    {
        var shop = await shops.GetByGuidAsync(cmd.ShopGuid, ct)
            ?? throw new NotFoundException("RamenShop", cmd.ShopGuid);

        shop.RemoveMenuItem(cmd.MenuItemId);
        await shops.UpdateAsync(shop, ct);
    }
}
