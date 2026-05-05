using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ramendo.Application.Common;
using Ramendo.Application.Reviews.Commands;
using Ramendo.Application.Reviews.DTOs;
using Ramendo.Domain.Aggregates.Reviews;

namespace Ramendo.Api.Controllers.Admin;

[ApiController]
[Route("api/admin/reviews")]
[Authorize(Roles = "Admin")]
public sealed class AdminReviewsController(IMediator mediator, IReviewRepository reviews) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ApiResponse<PagedResult<ReviewDto>>>> GetAll(
        [FromQuery] int page = 1, [FromQuery] int limit = 20, CancellationToken ct = default)
    {
        var (items, total) = await reviews.GetAllAsync(page, limit, ct);
        var dtos = items.Select(r => new ReviewDto(
            r.Id.ToString(), r.Rating, r.Content, [.. r.Images],
            r.UserId.ToString(), null, null, r.RamenShopId.ToString(), r.CreatedAt)).ToList();
        var result = PagedResult<ReviewDto>.Create(dtos, total, page, limit);
        return Ok(ApiResponse<PagedResult<ReviewDto>>.Ok(result));
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<ApiResponse>> Delete(Guid id, CancellationToken ct)
    {
        await mediator.Send(new DeleteReviewCommand(id, Guid.Empty, true), ct);
        return Ok(ApiResponse.Ok("Review deleted."));
    }
}
