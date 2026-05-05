using MediatR;
using Ramendo.Application.Shops.Commands;
using Ramendo.Application.Shops.DTOs;
using Ramendo.Domain.Aggregates.Shops;

namespace Ramendo.Application.Shops.Handlers;

public sealed class CreateShopCommandHandler(IRamenShopRepository shops)
    : IRequestHandler<CreateShopCommand, string>
{
    public async Task<string> Handle(CreateShopCommand cmd, CancellationToken ct)
    {
        var dto = cmd.Dto;
        var shop = RamenShop.Create(
            dto.Name, dto.Description, dto.City, dto.District,
            dto.DetailAddress, dto.Phone, dto.Website, dto.FacebookPageId, dto.Types);

        if (dto.BusinessHours is not null)
            shop.SetBusinessHours(MapBusinessHours(dto.BusinessHours));

        if (dto.NewsItems is not null)
            shop.SetNewsItems(dto.NewsItems.Select(MapNewsItem));

        await shops.AddAsync(shop, ct);
        return shop.Guid.ToString();
    }

    internal static BusinessHours MapBusinessHours(BusinessHoursDto bh) => new(
        bh.Monday is null ? null : MapDay(bh.Monday),
        bh.Tuesday is null ? null : MapDay(bh.Tuesday),
        bh.Wednesday is null ? null : MapDay(bh.Wednesday),
        bh.Thursday is null ? null : MapDay(bh.Thursday),
        bh.Friday is null ? null : MapDay(bh.Friday),
        bh.Saturday is null ? null : MapDay(bh.Saturday),
        bh.Sunday is null ? null : MapDay(bh.Sunday));

    internal static DayHours MapDay(DayHoursDto d) => new(
        d.IsOpen, d.IsSplit, d.Open, d.Close, d.LunchOpen, d.LunchClose, d.DinnerOpen, d.DinnerClose);

    internal static NewsItem MapNewsItem(NewsItemDto n) =>
        new(n.Title, n.Content, n.StartDate, n.EndDate, n.CreatedAt == default ? DateTime.UtcNow : n.CreatedAt);
}
