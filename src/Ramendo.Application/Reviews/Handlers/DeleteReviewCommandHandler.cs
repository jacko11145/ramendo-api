using MediatR;
using Ramendo.Application.Common;
using Ramendo.Application.Reviews.Commands;
using Ramendo.Domain.Aggregates.Reviews;

namespace Ramendo.Application.Reviews.Handlers;

public sealed class DeleteReviewCommandHandler(IReviewRepository reviews) : IRequestHandler<DeleteReviewCommand>
{
    public async Task Handle(DeleteReviewCommand cmd, CancellationToken ct)
    {
        var review = await reviews.GetByIdAsync(cmd.ReviewId, ct)
            ?? throw new NotFoundException("Review", cmd.ReviewId);

        if (!cmd.IsAdmin && review.UserId != cmd.RequestingUserId)
            throw new ForbiddenException("You cannot delete someone else's review.");

        await reviews.DeleteAsync(cmd.ReviewId, ct);
    }
}
