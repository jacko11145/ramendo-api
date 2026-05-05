using MediatR;

namespace Ramendo.Application.Reviews.Commands;

public sealed record DeleteReviewCommand(Guid ReviewId, Guid RequestingUserId, bool IsAdmin) : IRequest;
