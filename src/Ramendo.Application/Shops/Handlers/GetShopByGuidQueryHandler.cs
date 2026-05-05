using MediatR;
using Ramendo.Application.Common;
using Ramendo.Application.Shops.DTOs;
using Ramendo.Application.Shops.Queries;
using Ramendo.Domain.Aggregates.Shops;

namespace Ramendo.Application.Shops.Handlers;

public sealed class GetShopByGuidQueryHandler(IRamenShopRepository shops)
    : IRequestHandler<GetShopByGuidQuery, RamenShopDetailDto>
{
    public async Task<RamenShopDetailDto> Handle(GetShopByGuidQuery q, CancellationToken ct)
    {
        var shop = await shops.GetByGuidAsync(q.Guid, ct)
            ?? throw new NotFoundException("RamenShop", q.Guid);

        return ToDetailDto(shop);
    }

    internal static RamenShopDetailDto ToDetailDto(RamenShop s) => new(
        s.Guid.ToString(), s.Name, s.Description,
        s.City, s.District, s.DetailAddress, s.Phone, s.Website, s.FacebookPageId, s.Instagram,
        [.. s.Images], s.CoverImage, s.Rating, s.GoogleRating, s.CriticRating,
        s.ReviewCount, [.. s.Types], s.IsActive, s.IsVerified,
        s.BusinessHours is null ? null : MapBusinessHours(s.BusinessHours),
        s.NewsItems.Select(MapNewsItem).ToArray(),
        s.MenuItems.OrderBy(m => m.Position).Select(MapMenuItem).ToArray(),
        s.CreatedAt);

    private static BusinessHoursDto MapBusinessHours(BusinessHours bh) => new(
        bh.Monday is null ? null : MapDay(bh.Monday),
        bh.Tuesday is null ? null : MapDay(bh.Tuesday),
        bh.Wednesday is null ? null : MapDay(bh.Wednesday),
        bh.Thursday is null ? null : MapDay(bh.Thursday),
        bh.Friday is null ? null : MapDay(bh.Friday),
        bh.Saturday is null ? null : MapDay(bh.Saturday),
        bh.Sunday is null ? null : MapDay(bh.Sunday));

    private static DayHoursDto MapDay(DayHours d) => new(
        d.IsOpen, d.IsSplit, d.Open, d.Close, d.LunchOpen, d.LunchClose, d.DinnerOpen, d.DinnerClose);

    private static NewsItemDto MapNewsItem(NewsItem n) => new(n.Title, n.Content, n.StartDate, n.EndDate, n.CreatedAt);

    private static MenuItemDto MapMenuItem(MenuItem m) => new(
        m.Id.ToString(), m.Name, m.Price, m.Description, m.Category,
        m.CustomCategory, m.Image, m.IsHighlight, m.IsLimited, m.Status, m.Position,
        m.ItemOptions.Select(io => new MenuItemOptionGroupDto(
            io.OptionTypeId.ToString(), io.OptionType.Name, io.IsRequired,
            io.OptionValues.Select(ov => new MenuItemOptionValueDto(
                ov.OptionValueId.ToString(), ov.OptionValue.Value,
                ov.Price, ov.IsDefault)).ToArray())).ToArray());
}
