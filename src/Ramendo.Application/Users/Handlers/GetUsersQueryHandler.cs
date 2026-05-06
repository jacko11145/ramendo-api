using MediatR;
using Ramendo.Application.Common;
using Ramendo.Application.Users.DTOs;
using Ramendo.Application.Users.Queries;
using Ramendo.Domain.Aggregates.Users;

namespace Ramendo.Application.Users.Handlers;

public sealed class GetUsersQueryHandler(IUserRepository users)
    : IRequestHandler<GetUsersQuery, PagedResult<UserListDto>>
{
    public async Task<PagedResult<UserListDto>> Handle(GetUsersQuery q, CancellationToken ct)
    {
        var (items, total) = await users.GetPagedAsync(q.Page, q.Limit, q.Search, q.Role, ct);
        var dtos = items.Select(ToDto).ToList();
        return PagedResult<UserListDto>.Create(dtos, total, q.Page, q.Limit);
    }

    private static UserListDto ToDto(User u) => new(
        u.Id.ToString(), u.Name, u.Email, u.Image, u.Role.ToString(),
        u.IsActive, u.VIP.IsActive, u.VIP.MembershipExpiry,
        u.Experience.Points, u.Experience.Level, 0, u.CreatedAt);
}
