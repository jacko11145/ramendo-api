using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ramendo.Application.Common;
using Ramendo.Application.Rankings.DTOs;
using Ramendo.Application.Rankings.Queries;

namespace Ramendo.Api.Controllers;

[ApiController]
[Route("api/rankings")]
public sealed class RankingsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<RankingItemDto>>>> GetRankings(
        [FromQuery] string type = "user", CancellationToken ct = default)
    {
        var result = await mediator.Send(new GetRankingsQuery(type), ct);
        return Ok(ApiResponse<IReadOnlyList<RankingItemDto>>.Ok(result));
    }
}
