using MediatR;
using Ramendo.Application.Shops.DTOs;

namespace Ramendo.Application.Shops.Queries;

public sealed record GetShopByGuidQuery(Guid Guid) : IRequest<RamenShopDetailDto>;
