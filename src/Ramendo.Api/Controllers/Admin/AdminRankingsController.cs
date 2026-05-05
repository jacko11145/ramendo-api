using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ramendo.Application.Common;
using Ramendo.Application.Rankings.Commands;
using Ramendo.Application.Rankings.DTOs;

namespace Ramendo.Api.Controllers.Admin;

[ApiController]
[Route("api/admin/rankings")]
[Authorize(Roles = "Admin")]
public sealed class AdminRankingsController(IMediator mediator) : ControllerBase
{
    [HttpPut("settings")]
    public async Task<ActionResult<ApiResponse<RankingSettingsDto>>> UpdateSettings(
        [FromBody] RankingSettingsDto dto, CancellationToken ct)
    {
        var result = await mediator.Send(new UpdateRankingSettingsCommand(dto), ct);
        return Ok(ApiResponse<RankingSettingsDto>.Ok(result));
    }
}
