using MediatR;
using Ramendo.Application.Settings.DTOs;
using Ramendo.Application.Settings.Queries;
using Ramendo.Domain.Aggregates.InvitationCodes;
using Ramendo.Domain.Aggregates.Reviews;
using Ramendo.Domain.Aggregates.Shops;
using Ramendo.Domain.Aggregates.Submissions;
using Ramendo.Domain.Aggregates.Users;

namespace Ramendo.Application.Settings.Handlers;

public sealed class GetDashboardStatsQueryHandler(
    IUserRepository users,
    IRamenShopRepository shops,
    IReviewRepository reviews,
    IShopSubmissionRepository submissions,
    IInvitationCodeRepository invitationCodes) : IRequestHandler<GetDashboardStatsQuery, DashboardStatsDto>
{
    public async Task<DashboardStatsDto> Handle(GetDashboardStatsQuery q, CancellationToken ct)
    {
        var firstOfMonth = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1, 0, 0, 0, DateTimeKind.Utc);

        var totalUsers            = await users.CountAsync(ct);
        var totalShops            = await shops.CountAsync(ct);
        var totalReviews          = await reviews.CountAsync(ct);
        var pendingSubmissions    = await submissions.CountPendingAsync(ct);
        var activeInvitationCodes = await invitationCodes.CountActiveAsync(ct);
        var newUsersThisMonth     = await users.CountSinceAsync(firstOfMonth, ct);
        var newReviewsThisMonth   = await reviews.CountSinceAsync(firstOfMonth, ct);

        return new DashboardStatsDto(
            totalUsers, totalShops, totalReviews,
            pendingSubmissions, activeInvitationCodes,
            newUsersThisMonth, newReviewsThisMonth);
    }
}
