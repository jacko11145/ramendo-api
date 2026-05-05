using MediatR;
using Ramendo.Application.Common;
using Ramendo.Application.Shops.Commands;
using Ramendo.Domain.Aggregates.Shops;

namespace Ramendo.Application.Shops.Handlers;

public sealed class AddMenuItemCommandHandler(IRamenShopRepository shops) : IRequestHandler<AddMenuItemCommand, string>
{
    public async Task<string> Handle(AddMenuItemCommand cmd, CancellationToken ct)
    {
        var shop = await shops.GetByGuidAsync(cmd.ShopGuid, ct)
            ?? throw new NotFoundException("RamenShop", cmd.ShopGuid);

        var item = shop.AddMenuItem(cmd.Name, cmd.Price, cmd.Description, cmd.Category,
            cmd.CustomCategory, cmd.IsHighlight, cmd.IsLimited, cmd.Position);

        await shops.UpdateAsync(shop, ct);
        return item.Id.ToString();
    }
}
