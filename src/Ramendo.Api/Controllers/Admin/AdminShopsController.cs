using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ramendo.Application.Common;
using Ramendo.Application.Shops.Commands;
using Ramendo.Application.Shops.DTOs;
using Ramendo.Application.Shops.Queries;
using Ramendo.Domain.Aggregates.Shops;

namespace Ramendo.Api.Controllers.Admin;

[ApiController]
[Route("api/admin/shops")]
//[Authorize(Roles = "Admin")]
public sealed class AdminShopsController(IMediator mediator, IRamenShopRepository shops, IImageUploadService images) : ControllerBase
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

    [HttpPost("{guid:guid}/cover-image")]
    [Consumes("multipart/form-data")]
    public async Task<ActionResult<ApiResponse<string>>> UploadCoverImage(
        Guid guid, IFormFile file, CancellationToken ct)
    {
        var shop = await shops.GetByGuidAsync(guid, ct)
            ?? throw new NotFoundException("RamenShop", guid);

        await using var stream = file.OpenReadStream();
        var url = await images.UploadAsync(stream, file.FileName, $"ramendo/shops/{guid}/cover", ct);

        if (shop.CoverImage is not null)
            await images.DeleteAsync(shop.CoverImage, ct);

        shop.SetCoverImage(url);
        await shops.UpdateAsync(shop, ct);
        return Ok(ApiResponse<string>.Ok(url));
    }

    [HttpDelete("{guid:guid}/cover-image")]
    public async Task<ActionResult<ApiResponse>> DeleteCoverImage(Guid guid, CancellationToken ct)
    {
        var shop = await shops.GetByGuidAsync(guid, ct)
            ?? throw new NotFoundException("RamenShop", guid);

        if (shop.CoverImage is not null)
        {
            await images.DeleteAsync(shop.CoverImage, ct);
            shop.SetCoverImage(null);
            await shops.UpdateAsync(shop, ct);
        }
        return Ok(ApiResponse.Ok("Cover image removed."));
    }

    [HttpPost("{guid:guid}/gallery")]
    [Consumes("multipart/form-data")]
    public async Task<ActionResult<ApiResponse<string[]>>> UploadGalleryImages(
        Guid guid, IFormFileCollection files, CancellationToken ct)
    {
        var shop = await shops.GetByGuidAsync(guid, ct)
            ?? throw new NotFoundException("RamenShop", guid);

        var urls = new List<string>();
        foreach (var file in files)
        {
            await using var stream = file.OpenReadStream();
            var url = await images.UploadAsync(stream, file.FileName, $"ramendo/shops/{guid}/gallery", ct);
            urls.Add(url);
        }

        shop.SetImages([.. shop.Images, .. urls]);
        await shops.UpdateAsync(shop, ct);
        return Ok(ApiResponse<string[]>.Ok([.. shop.Images]));
    }

    [HttpDelete("{guid:guid}/gallery")]
    public async Task<ActionResult<ApiResponse<string[]>>> DeleteGalleryImage(
        Guid guid, [FromBody] DeleteGalleryImageRequest req, CancellationToken ct)
    {
        var shop = await shops.GetByGuidAsync(guid, ct)
            ?? throw new NotFoundException("RamenShop", guid);

        if (shop.Images.Contains(req.Url))
        {
            await images.DeleteAsync(req.Url, ct);
            shop.SetImages(shop.Images.Where(u => u != req.Url));
            await shops.UpdateAsync(shop, ct);
        }
        return Ok(ApiResponse<string[]>.Ok([.. shop.Images]));
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

    [HttpPost("{guid:guid}/menu/{itemId:guid}/image")]
    [Consumes("multipart/form-data")]
    public async Task<ActionResult<ApiResponse<string>>> UploadMenuItemImage(
        Guid guid, Guid itemId, IFormFile file, CancellationToken ct)
    {
        var shop = await shops.GetByGuidAsync(guid, ct)
            ?? throw new NotFoundException("RamenShop", guid);

        var item = await shops.GetMenuItemAsync(shop.Id, itemId, ct)
            ?? throw new NotFoundException("MenuItem", itemId);

        await using var stream = file.OpenReadStream();
        var url = await images.UploadAsync(stream, file.FileName, $"ramendo/shops/{guid}/menu", ct);

        if (item.Image is not null)
            await images.DeleteAsync(item.Image, ct);

        item.SetImage(url);
        await shops.UpdateMenuItemAsync(item, ct);
        return Ok(ApiResponse<string>.Ok(url));
    }

    [HttpDelete("{guid:guid}/menu/{itemId:guid}/image")]
    public async Task<ActionResult<ApiResponse>> DeleteMenuItemImage(
        Guid guid, Guid itemId, CancellationToken ct)
    {
        var shop = await shops.GetByGuidAsync(guid, ct)
            ?? throw new NotFoundException("RamenShop", guid);

        var item = await shops.GetMenuItemAsync(shop.Id, itemId, ct)
            ?? throw new NotFoundException("MenuItem", itemId);

        if (item.Image is not null)
        {
            await images.DeleteAsync(item.Image, ct);
            item.SetImage(null);
            await shops.UpdateMenuItemAsync(item, ct);
        }
        return Ok(ApiResponse.Ok("Menu item image removed."));
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

public sealed record DeleteGalleryImageRequest(string Url);
