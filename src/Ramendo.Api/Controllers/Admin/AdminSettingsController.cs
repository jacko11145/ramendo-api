using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ramendo.Application.Common;
using Ramendo.Application.Rankings.Commands;
using Ramendo.Application.Rankings.DTOs;
using Ramendo.Application.Settings.Commands;
using Ramendo.Application.Settings.DTOs;
using Ramendo.Application.Settings.Queries;

namespace Ramendo.Api.Controllers.Admin;

[ApiController]
[Route("api/admin")]
[Authorize(Roles = "Admin")]
public sealed class AdminSettingsController(IMediator mediator) : ControllerBase
{
    [HttpGet("settings/permissions")]
    public async Task<ActionResult<ApiResponse<PermissionSettingsDto>>> GetPermissions(CancellationToken ct)
    {
        var result = await mediator.Send(new GetPermissionSettingsQuery(), ct);
        return Ok(ApiResponse<PermissionSettingsDto>.Ok(result));
    }

    [HttpPut("settings/permissions")]
    public async Task<ActionResult<ApiResponse<PermissionSettingsDto>>> UpdatePermissions(
        [FromBody] PermissionSettingsDto dto, CancellationToken ct)
    {
        var result = await mediator.Send(new UpdatePermissionSettingsCommand(dto), ct);
        return Ok(ApiResponse<PermissionSettingsDto>.Ok(result));
    }

    [HttpGet("dashboard/stats")]
    public async Task<ActionResult<ApiResponse<DashboardStatsDto>>> GetStats(CancellationToken ct)
    {
        var result = await mediator.Send(new GetDashboardStatsQuery(), ct);
        return Ok(ApiResponse<DashboardStatsDto>.Ok(result));
    }
}
