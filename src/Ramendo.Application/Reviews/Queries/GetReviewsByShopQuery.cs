using MediatR;
using Ramendo.Application.Common;
using Ramendo.Application.Reviews.DTOs;

namespace Ramendo.Application.Reviews.Queries;

public sealed record GetReviewsByShopQuery(Guid ShopGuid, int Page = 1, int Limit = 20)
    : IRequest<PagedResult<ReviewDto>>;
