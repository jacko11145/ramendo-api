using MediatR;
using Ramendo.Application.Common;
using Ramendo.Application.Reviews.DTOs;
using Ramendo.Application.Reviews.Queries;
using Ramendo.Domain.Aggregates.Reviews;
using Ramendo.Domain.Aggregates.Shops;

namespace Ramendo.Application.Reviews.Handlers;

public sealed class GetReviewsByShopQueryHandler(
    IReviewRepository reviews,
    IRamenShopRepository shops) : IRequestHandler<GetReviewsByShopQuery, PagedResult<ReviewDto>>
{
    public async Task<PagedResult<ReviewDto>> Handle(GetReviewsByShopQuery q, CancellationToken ct)
    {
        var shop = await shops.GetByGuidAsync(q.ShopGuid, ct)
            ?? throw new NotFoundException("RamenShop", q.ShopGuid);

        (IReadOnlyList<Review> items, int total) = await reviews.GetByShopAsync(shop.Id, q.Page, q.Limit, ct);
        var dtos = items.Select(ToDto).ToList();
        return PagedResult<ReviewDto>.Create(dtos, total, q.Page, q.Limit);
    }

    private static ReviewDto ToDto(Review r) => new(
        r.Id.ToString(), r.Rating, r.Content,
        r.User?.Experience.Level ?? 1,
        r.VisitDate?.ToString("yyyy-MM-dd"),
        r.UserId.ToString(), r.User?.Name, r.User?.Image,
        r.RamenShopId.ToString(), r.CreatedAt, [.. r.Images]);
}
