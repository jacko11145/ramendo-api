using MediatR;
using Ramendo.Application.Common;
using Ramendo.Application.Users.DTOs;

namespace Ramendo.Application.Users.Queries;

public sealed record GetUsersQuery(int Page = 1, int Limit = 20, string? Search = null, string? Role = null)
    : IRequest<PagedResult<UserListDto>>;
