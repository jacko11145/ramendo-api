using MediatR;
using Ramendo.Application.Common;
using Ramendo.Application.Shops.Commands;
using Ramendo.Domain.Aggregates.Shops;

namespace Ramendo.Application.Shops.Handlers;

public sealed class UpdateShopCommandHandler(IRamenShopRepository shops) : IRequestHandler<UpdateShopCommand>
{
    public async Task Handle(UpdateShopCommand cmd, CancellationToken ct)
    {
        var shop = await shops.GetByGuidAsync(cmd.Guid, ct)
            ?? throw new NotFoundException("RamenShop", cmd.Guid);

        var dto = cmd.Dto;
        shop.Update(dto.Name, dto.Description, dto.City, dto.District,
            dto.DetailAddress, dto.Phone, dto.Website, dto.FacebookPageId, dto.Instagram, dto.Types, dto.IsActive);

        shop.SetVerified(dto.IsVerified);
        if (dto.GoogleRating.HasValue) shop.SetGoogleRating(dto.GoogleRating.Value);

        shop.SetBusinessHours(dto.BusinessHours is null ? null : CreateShopCommandHandler.MapBusinessHours(dto.BusinessHours));
        shop.SetNewsItems(dto.NewsItems?.Select(CreateShopCommandHandler.MapNewsItem) ?? []);

        await shops.UpdateAsync(shop, ct);
    }
}
