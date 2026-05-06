using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ramendo.Application.Common;
using Ramendo.Application.Reviews.Commands;
using Ramendo.Application.Reviews.DTOs;
using Ramendo.Domain.Aggregates.Reviews;
using Ramendo.Domain.Aggregates.Shops;

namespace Ramendo.Api.Controllers.Admin;

[ApiController]
[Route("api/admin/reviews")]
[Authorize(Roles = "Admin")]
public sealed class AdminReviewsController(IMediator mediator, IReviewRepository reviews, IRamenShopRepository shops) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ApiResponse<PagedResult<AdminReviewDto>>>> GetAll(
        [FromQuery] int page = 1, [FromQuery] int limit = 20, CancellationToken ct = default)
    {
        var (items, total) = await reviews.GetAllAsync(page, limit, ct);
        var shopIds = items.Select(r => r.RamenShopId).Distinct();
        var shopMap = (await shops.GetManyByIdsAsync(shopIds, ct)).ToDictionary(s => s.Id, s => s.Name);
        var dtos = items.Select(r => new AdminReviewDto(
            r.Id.ToString(), r.Rating, r.Content,
            r.VisitDate?.ToString("yyyy-MM-dd"),
            r.UserId.ToString(), r.User?.Name,
            r.RamenShopId.ToString(), shopMap.GetValueOrDefault(r.RamenShopId),
            r.CreatedAt)).ToList();
        var result = PagedResult<AdminReviewDto>.Create(dtos, total, page, limit);
        return Ok(ApiResponse<PagedResult<AdminReviewDto>>.Ok(result));
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<ApiResponse>> Delete(Guid id, CancellationToken ct)
    {
        await mediator.Send(new DeleteReviewCommand(id, Guid.Empty, true), ct);
        return Ok(ApiResponse.Ok("Review deleted."));
    }
}
