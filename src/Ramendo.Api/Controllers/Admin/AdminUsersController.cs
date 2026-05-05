using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ramendo.Application.Common;
using Ramendo.Application.Users.Commands;
using Ramendo.Application.Users.DTOs;
using Ramendo.Application.Users.Queries;

namespace Ramendo.Api.Controllers.Admin;

[ApiController]
[Route("api/admin/users")]
[Authorize(Roles = "Admin")]
public sealed class AdminUsersController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ApiResponse<PagedResult<UserListDto>>>> GetUsers(
        [FromQuery] int page = 1, [FromQuery] int limit = 20,
        [FromQuery] string? search = null, [FromQuery] string? role = null,
        CancellationToken ct = default)
    {
        var result = await mediator.Send(new GetUsersQuery(page, limit, search, role), ct);
        return Ok(ApiResponse<PagedResult<UserListDto>>.Ok(result));
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<ApiResponse>> DeleteUser(Guid id, CancellationToken ct)
    {
        await mediator.Send(new DeleteUserCommand(id), ct);
        return Ok(ApiResponse.Ok("User deleted."));
    }

    [HttpPut("{id:guid}/role")]
    public async Task<ActionResult<ApiResponse>> UpdateRole(Guid id, [FromBody] UpdateUserRoleDto dto, CancellationToken ct)
    {
        await mediator.Send(new UpdateUserRoleCommand(id, dto.Role), ct);
        return Ok(ApiResponse.Ok());
    }

    [HttpPut("{id:guid}/status")]
    public async Task<ActionResult<ApiResponse>> UpdateStatus(Guid id, [FromBody] bool isActive, CancellationToken ct)
    {
        await mediator.Send(new UpdateUserStatusCommand(id, isActive), ct);
        return Ok(ApiResponse.Ok());
    }

    [HttpPut("{id:guid}/vip")]
    public async Task<ActionResult<ApiResponse>> SetVip(Guid id, [FromBody] SetVipDto dto, CancellationToken ct)
    {
        await mediator.Send(new SetVipMembershipCommand(id, dto.IsVIP, dto.MembershipExpiry), ct);
        return Ok(ApiResponse.Ok());
    }

    [HttpPut("{id:guid}/experience")]
    public async Task<ActionResult<ApiResponse>> AdjustExperience(Guid id, [FromBody] AdjustExperienceDto dto, CancellationToken ct)
    {
        await mediator.Send(new AdjustExperienceCommand(id, dto.Points), ct);
        return Ok(ApiResponse.Ok());
    }
}
