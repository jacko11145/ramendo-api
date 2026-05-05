using MediatR;
using Ramendo.Application.Common;
using Ramendo.Application.Shops.Commands;
using Ramendo.Domain.Aggregates.Shops;

namespace Ramendo.Application.Shops.Handlers;

public sealed class ReorderMenuItemsCommandHandler(IRamenShopRepository shops) : IRequestHandler<ReorderMenuItemsCommand>
{
    public async Task Handle(ReorderMenuItemsCommand cmd, CancellationToken ct)
    {
        var shop = await shops.GetByGuidAsync(cmd.ShopGuid, ct)
            ?? throw new NotFoundException("RamenShop", cmd.ShopGuid);

        shop.ReorderMenuItems(cmd.Order.Select(o => (o.Id, o.Position)));
        await shops.UpdateAsync(shop, ct);
    }
}
