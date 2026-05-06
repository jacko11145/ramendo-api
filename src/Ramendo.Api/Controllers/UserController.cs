using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ramendo.Application.Auth.DTOs;
using Ramendo.Application.Common;
using Ramendo.Application.Submissions.DTOs;
using Ramendo.Application.Submissions.Queries;
using Ramendo.Domain.Aggregates.Users;

namespace Ramendo.Api.Controllers;

[ApiController]
[Route("api/user")]
[Authorize]
public sealed class UserController(IUserRepository users, IMediator mediator) : ControllerBase
{
    [HttpGet("profile")]
    public async Task<ActionResult<ApiResponse<UserSessionDto>>> GetProfile(CancellationToken ct)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var user = await users.GetByIdAsync(userId, ct)
            ?? throw new NotFoundException("User", userId);

        var dto = new UserSessionDto(
            user.Id.ToString(), user.Email, user.Name, user.Image,
            user.Role.ToString(), user.Experience.Points, user.Experience.Level,
            user.VIP.IsActive, user.VIP.MembershipExpiry);

        return Ok(ApiResponse<UserSessionDto>.Ok(dto));
    }

    [HttpGet("submissions")]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<ShopSubmissionDto>>>> GetMySubmissions(CancellationToken ct)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result = await mediator.Send(new GetUserSubmissionsQuery(userId), ct);
        return Ok(ApiResponse<IReadOnlyList<ShopSubmissionDto>>.Ok(result));
    }
}
