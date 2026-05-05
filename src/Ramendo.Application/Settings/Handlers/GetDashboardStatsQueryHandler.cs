using MediatR;
using Ramendo.Application.Settings.DTOs;
using Ramendo.Application.Settings.Queries;
using Ramendo.Domain.Aggregates.InvitationCodes;
using Ramendo.Domain.Aggregates.Reviews;
using Ramendo.Domain.Aggregates.Shops;
using Ramendo.Domain.Aggregates.Users;

namespace Ramendo.Application.Settings.Handlers;

public sealed class GetDashboardStatsQueryHandler(
    IUserRepository users,
    IRamenShopRepository shops,
    IReviewRepository reviews) : IRequestHandler<GetDashboardStatsQuery, DashboardStatsDto>
{
    public async Task<DashboardStatsDto> Handle(GetDashboardStatsQuery q, CancellationToken ct)
    {
        var totalUsers = await users.CountAsync(ct);
        var totalShops = await shops.CountAsync(ct);
        var totalReviews = await reviews.CountAsync(ct);

        return new DashboardStatsDto(totalUsers, totalShops, 0, totalReviews, 0);
    }
}
