using MediatR;
using Ramendo.Application.Reviews.DTOs;

namespace Ramendo.Application.Reviews.Commands;

public sealed record CreateReviewCommand(Guid UserId, CreateReviewDto Dto) : IRequest<ReviewDto>;
