using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ramendo.Application.Common;
using Ramendo.Application.Shops.DTOs;
using Ramendo.Application.Shops.Queries;

namespace Ramendo.Api.Controllers;

[ApiController]
[Route("api/shops")]
public sealed class ShopsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ApiResponse<PagedResult<RamenShopListDto>>>> GetShops(
        [FromQuery] string? city, [FromQuery] string? district, [FromQuery] string[]? types,
        [FromQuery] string sort = "rating", [FromQuery] int page = 1, [FromQuery] int limit = 20,
        CancellationToken ct = default)
    {
        var result = await mediator.Send(new GetShopsQuery(city, district, types, sort, page, limit), ct);
        return Ok(ApiResponse<PagedResult<RamenShopListDto>>.Ok(result));
    }

    [HttpGet("{guid:guid}")]
    public async Task<ActionResult<ApiResponse<RamenShopDetailDto>>> GetShop(Guid guid, CancellationToken ct)
    {
        var result = await mediator.Send(new GetShopByGuidQuery(guid), ct);
        return Ok(ApiResponse<RamenShopDetailDto>.Ok(result));
    }
}
