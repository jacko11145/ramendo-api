using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ramendo.Application.Common;
using Ramendo.Application.Shops.Commands;
using Ramendo.Application.Shops.DTOs;
using Ramendo.Application.Shops.Queries;

namespace Ramendo.Api.Controllers.Admin;

[ApiController]
[Route("api/admin/shops")]
[Authorize(Roles = "Admin")]
public sealed class AdminShopsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ApiResponse<PagedResult<RamenShopListDto>>>> GetShops(
        [FromQuery] int page = 1, [FromQuery] int limit = 20, [FromQuery] string? search = null, CancellationToken ct = default)
    {
        var result = await mediator.Send(new GetShopsQuery(null, null, null, "newest", page, limit, search, AdminMode: true), ct);
        return Ok(ApiResponse<PagedResult<RamenShopListDto>>.Ok(result));
    }

    [HttpGet("{guid:guid}")]
    public async Task<ActionResult<ApiResponse<RamenShopDetailDto>>> GetShop(Guid guid, CancellationToken ct)
    {
        var result = await mediator.Send(new GetShopByGuidQuery(guid), ct);
        return Ok(ApiResponse<RamenShopDetailDto>.Ok(result));
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<string>>> CreateShop(
        [FromBody] CreateUpdateShopDto dto, CancellationToken ct)
    {
        var id = await mediator.Send(new CreateShopCommand(dto), ct);
        return Ok(ApiResponse<string>.Ok(id, "Shop created."));
    }

    [HttpPut("{guid:guid}")]
    public async Task<ActionResult<ApiResponse>> UpdateShop(
        Guid guid, [FromBody] CreateUpdateShopDto dto, CancellationToken ct)
    {
        await mediator.Send(new UpdateShopCommand(guid, dto), ct);
        return Ok(ApiResponse.Ok("Shop updated."));
    }

    [HttpDelete("{guid:guid}")]
    public async Task<ActionResult<ApiResponse>> DeleteShop(Guid guid, CancellationToken ct)
    {
        await mediator.Send(new DeleteShopCommand(guid), ct);
        return Ok(ApiResponse.Ok("Shop deleted."));
    }

    [HttpPost("{guid:guid}/menu")]
    public async Task<ActionResult<ApiResponse<string>>> AddMenuItem(
        Guid guid, [FromBody] AddMenuItemRequest req, CancellationToken ct)
    {
        var id = await mediator.Send(new AddMenuItemCommand(
            guid, req.Name, req.Price, req.Description, req.Category,
            req.CustomCategory, req.IsHighlight, req.IsLimited, req.Position), ct);
        return Ok(ApiResponse<string>.Ok(id, "Menu item added."));
    }

    [HttpPut("{guid:guid}/menu/reorder")]
    public async Task<ActionResult<ApiResponse>> ReorderMenuItems(
        Guid guid, [FromBody] ReorderMenuItemsRequest req, CancellationToken ct)
    {
        var order = req.Order.Select(o => new ReorderItem(o.Id, o.Position)).ToList();
        await mediator.Send(new ReorderMenuItemsCommand(guid, order), ct);
        return Ok(ApiResponse.Ok("Menu reordered."));
    }

    [HttpDelete("{guid:guid}/menu/{itemId:guid}")]
    public async Task<ActionResult<ApiResponse>> DeleteMenuItem(
        Guid guid, Guid itemId, CancellationToken ct)
    {
        await mediator.Send(new DeleteMenuItemCommand(guid, itemId), ct);
        return Ok(ApiResponse.Ok("Menu item deleted."));
    }
}
