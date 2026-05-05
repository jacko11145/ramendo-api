using MediatR;
using Ramendo.Application.Common;
using Ramendo.Application.Shops.Commands;
using Ramendo.Domain.Aggregates.Shops;

namespace Ramendo.Application.Shops.Handlers;

public sealed class DeleteShopCommandHandler(IRamenShopRepository shops) : IRequestHandler<DeleteShopCommand>
{
    public async Task Handle(DeleteShopCommand cmd, CancellationToken ct)
    {
        var shop = await shops.GetByGuidAsync(cmd.Guid, ct)
            ?? throw new NotFoundException("RamenShop", cmd.Guid);

        await shops.DeleteAsync(shop.Id, ct);
    }
}
