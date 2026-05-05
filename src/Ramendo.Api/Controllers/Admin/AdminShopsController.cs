using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ramendo.Application.Common;
using Ramendo.Application.Shops.Commands;
using Ramendo.Application.Shops.DTOs;

namespace Ramendo.Api.Controllers.Admin;

[ApiController]
[Route("api/admin/shops")]
[Authorize(Roles = "Admin")]
public sealed class AdminShopsController(IMediator mediator) : ControllerBase
{
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
}
