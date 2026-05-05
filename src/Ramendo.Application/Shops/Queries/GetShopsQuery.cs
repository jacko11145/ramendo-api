using MediatR;
using Ramendo.Application.Common;
using Ramendo.Application.Shops.DTOs;

namespace Ramendo.Application.Shops.Queries;

public sealed record GetShopsQuery(
    string? City, string? District, string[]? Types, string Sort = "rating",
    int Page = 1, int Limit = 20,
    string? Search = null, bool AdminMode = false) : IRequest<PagedResult<RamenShopListDto>>;
