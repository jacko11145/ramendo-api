using MediatR;
using Ramendo.Application.Reviews.Commands;
using Ramendo.Application.Reviews.DTOs;
using Ramendo.Domain.Aggregates.Reviews;

namespace Ramendo.Application.Reviews.Handlers;

public sealed class RateMenuItemCommandHandler(IReviewRepository reviews) : IRequestHandler<RateMenuItemCommand, MenuItemRatingDto>
{
    public async Task<MenuItemRatingDto> Handle(RateMenuItemCommand cmd, CancellationToken ct)
    {
        var existing = await reviews.GetMenuItemRatingAsync(cmd.UserId, cmd.MenuItemId, ct);

        if (existing is not null)
        {
            var newOptions = cmd.Dto.SelectedOptionValueIds
                .Select(id => ReviewOption.Create(existing.Id, Guid.Parse(id)));
            existing.Update(cmd.Dto.Rating, cmd.Dto.Comment, newOptions);
            await reviews.UpdateMenuItemRatingAsync(existing, ct);
            return ToDto(existing);
        }

        var rating = MenuItemRating.Create(cmd.UserId, cmd.MenuItemId, cmd.Dto.Rating, cmd.Dto.Comment);
        foreach (var optId in cmd.Dto.SelectedOptionValueIds)
            rating.AddReviewOption(ReviewOption.Create(rating.Id, Guid.Parse(optId)));

        await reviews.AddMenuItemRatingAsync(rating, ct);
        return ToDto(rating);
    }

    private static MenuItemRatingDto ToDto(MenuItemRating r) => new(
        r.Id.ToString(), r.Rating, r.Comment, r.UserId.ToString(), r.MenuItemId.ToString(),
        r.ReviewOptions.Select(o => o.OptionValueId.ToString()).ToArray(), r.CreatedAt);
}
