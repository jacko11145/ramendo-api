using MediatR;
using Ramendo.Application.Common;
using Ramendo.Application.Reviews.Commands;
using Ramendo.Application.Reviews.DTOs;
using Ramendo.Domain.Aggregates.Reviews;
using Ramendo.Domain.Aggregates.Shops;
using Ramendo.Domain.Aggregates.Users;
using Ramendo.Domain.Services;

namespace Ramendo.Application.Reviews.Handlers;

public sealed class CreateReviewCommandHandler(
    IReviewRepository reviews,
    IRamenShopRepository shops,
    IUserRepository users,
    IPermissionService permissions) : IRequestHandler<CreateReviewCommand, ReviewDto>
{
    public async Task<ReviewDto> Handle(CreateReviewCommand cmd, CancellationToken ct)
    {
        var shop = await shops.GetByGuidAsync(cmd.Dto.ShopGuid, ct)
            ?? throw new NotFoundException("RamenShop", cmd.Dto.ShopGuid);

        var user = await users.GetByIdAsync(cmd.UserId, ct)
            ?? throw new NotFoundException("User", cmd.UserId);

        if (user.Role == UserRole.User && !await permissions.CanReviewAsync(user.Experience.Level, ct))
            throw new ForbiddenException("您的等級不足以撰寫評論。");

        var review = Review.Create(cmd.UserId, shop.Id, cmd.Dto.Rating, cmd.Dto.Comment, cmd.Dto.Images ?? [], cmd.Dto.VisitDate);
        await reviews.AddAsync(review, ct);

        var (avg, count) = await reviews.GetRatingStatsAsync(shop.Id, ct);
        shop.UpdateRating(avg, count);
        await shops.UpdateAsync(shop, ct);

        return new ReviewDto(
            review.Id.ToString(), review.Rating, review.Content,
            1, review.VisitDate?.ToString("yyyy-MM-dd"),
            review.UserId.ToString(), null, null,
            review.RamenShopId.ToString(), review.CreatedAt);
    }
}
