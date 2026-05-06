using MediatR;
using Ramendo.Application.Common;
using Ramendo.Application.Reviews.Commands;
using Ramendo.Domain.Aggregates.Reviews;
using Ramendo.Domain.Aggregates.Shops;

namespace Ramendo.Application.Reviews.Handlers;

public sealed class DeleteReviewCommandHandler(
    IReviewRepository reviews,
    IRamenShopRepository shops) : IRequestHandler<DeleteReviewCommand>
{
    public async Task Handle(DeleteReviewCommand cmd, CancellationToken ct)
    {
        var review = await reviews.GetByIdAsync(cmd.ReviewId, ct)
            ?? throw new NotFoundException("Review", cmd.ReviewId);

        if (!cmd.IsAdmin && review.UserId != cmd.RequestingUserId)
            throw new ForbiddenException("You cannot delete someone else's review.");

        var shopId = review.RamenShopId;
        await reviews.DeleteAsync(cmd.ReviewId, ct);

        var shop = await shops.GetByIdAsync(shopId, ct);
        if (shop is not null)
        {
            var (avg, count) = await reviews.GetRatingStatsAsync(shopId, ct);
            shop.UpdateRating(avg, count);
            await shops.UpdateAsync(shop, ct);
        }
    }
}
