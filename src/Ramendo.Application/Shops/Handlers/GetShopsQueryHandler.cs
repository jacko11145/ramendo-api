using MediatR;
using Ramendo.Application.Common;
using Ramendo.Application.Shops.DTOs;
using Ramendo.Application.Shops.Queries;
using Ramendo.Domain.Aggregates.Shops;

namespace Ramendo.Application.Shops.Handlers;

public sealed class GetShopsQueryHandler(IRamenShopRepository shops)
    : IRequestHandler<GetShopsQuery, PagedResult<RamenShopListDto>>
{
    public async Task<PagedResult<RamenShopListDto>> Handle(GetShopsQuery q, CancellationToken ct)
    {
        var (items, total) = await shops.GetPagedAsync(q.City, q.District, q.Types, q.Sort, q.Page, q.Limit, q.Search, q.AdminMode, ct);
        var dtos = items.Select(ToListDto).ToList();
        return PagedResult<RamenShopListDto>.Create(dtos, total, q.Page, q.Limit);
    }

    private static RamenShopListDto ToListDto(RamenShop s) => new(
        s.Guid.ToString(), s.Name, s.City, s.District, s.DetailAddress,
        s.CoverImage, s.Rating, s.GoogleRating, s.ReviewCount,
        [.. s.Types], s.IsActive, s.IsVerified, s.Phone, s.Instagram);
}
