using MediatR;
using Ramendo.Application.Rankings.DTOs;
using Ramendo.Application.Rankings.Queries;
using Ramendo.Domain.Aggregates.Shops;
using Ramendo.Domain.Services;

namespace Ramendo.Application.Rankings.Handlers;

public sealed class GetRankingsQueryHandler(
    IRamenShopRepository shops,
    IRankingService ranking) : IRequestHandler<GetRankingsQuery, IReadOnlyList<RankingItemDto>>
{
    public async Task<IReadOnlyList<RankingItemDto>> Handle(GetRankingsQuery q, CancellationToken ct)
    {
        var all = await shops.GetForRankingsAsync(ct);
        var ranked = ranking.Rank(all, q.Type);

        return ranked.Select((x, i) => new RankingItemDto(
            i + 1,
            x.Shop.Guid.ToString(),
            x.Shop.Name,
            x.Shop.City,
            x.Shop.District,
            x.Shop.CoverImage,
            x.Score,
            x.Shop.Rating,
            x.Shop.GoogleRating,
            x.Shop.ReviewCount,
            [.. x.Shop.Types])).ToList();
    }
}
