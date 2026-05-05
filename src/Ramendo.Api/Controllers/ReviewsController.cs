using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ramendo.Application.Common;
using Ramendo.Application.Reviews.Commands;
using Ramendo.Application.Reviews.DTOs;
using Ramendo.Application.Reviews.Queries;
using Ramendo.Application.Shops.Queries;

namespace Ramendo.Api.Controllers;

[ApiController]
[Route("api")]
public sealed class ReviewsController(IMediator mediator) : ControllerBase
{
    [HttpGet("shops/{guid:guid}/reviews")]
    public async Task<ActionResult<ApiResponse<PagedResult<ReviewDto>>>> GetByShop(
        Guid guid, [FromQuery] int page = 1, [FromQuery] int limit = 20, CancellationToken ct = default)
    {
        var result = await mediator.Send(new GetReviewsByShopQuery(guid, page, limit), ct);
        return Ok(ApiResponse<PagedResult<ReviewDto>>.Ok(result));
    }

    [HttpPost("reviews")]
    [Authorize]
    public async Task<ActionResult<ApiResponse<ReviewDto>>> Create(
        [FromBody] CreateReviewDto dto, CancellationToken ct)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result = await mediator.Send(new CreateReviewCommand(userId, dto), ct);
        return Ok(ApiResponse<ReviewDto>.Ok(result, "Review created."));
    }

    [HttpDelete("reviews/{id:guid}")]
    [Authorize]
    public async Task<ActionResult<ApiResponse>> Delete(Guid id, CancellationToken ct)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var isAdmin = User.IsInRole("Admin");
        await mediator.Send(new DeleteReviewCommand(id, userId, isAdmin), ct);
        return Ok(ApiResponse.Ok("Review deleted."));
    }

    [HttpPost("menu-items/{menuItemId:guid}/ratings")]
    [Authorize]
    public async Task<ActionResult<ApiResponse<MenuItemRatingDto>>> RateMenuItem(
        Guid menuItemId, [FromBody] CreateMenuItemRatingDto dto, CancellationToken ct)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result = await mediator.Send(new RateMenuItemCommand(userId, menuItemId, dto), ct);
        return Ok(ApiResponse<MenuItemRatingDto>.Ok(result));
    }
}
