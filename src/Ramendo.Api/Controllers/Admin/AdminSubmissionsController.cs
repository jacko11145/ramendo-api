using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ramendo.Application.Common;
using Ramendo.Application.Submissions.Commands;
using Ramendo.Application.Submissions.DTOs;
using Ramendo.Application.Submissions.Queries;

namespace Ramendo.Api.Controllers.Admin;

[ApiController]
[Route("api/admin/submissions")]
[Authorize(Roles = "Admin")]
public sealed class AdminSubmissionsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ApiResponse<PagedResult<ShopSubmissionDto>>>> GetAll(
        [FromQuery] int page = 1, [FromQuery] int limit = 20, CancellationToken ct = default)
    {
        var result = await mediator.Send(new GetSubmissionsQuery(page, limit), ct);
        return Ok(ApiResponse<PagedResult<ShopSubmissionDto>>.Ok(result));
    }

    [HttpPut("{id:guid}/approve")]
    public async Task<ActionResult<ApiResponse<string>>> Approve(Guid id, CancellationToken ct)
    {
        var adminId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var shopGuid = await mediator.Send(new ApproveSubmissionCommand(id, adminId), ct);
        return Ok(ApiResponse<string>.Ok(shopGuid, "Submission approved. Shop created."));
    }

    [HttpPut("{id:guid}/reject")]
    public async Task<ActionResult<ApiResponse>> Reject(Guid id, [FromBody] string? feedback, CancellationToken ct)
    {
        var adminId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        await mediator.Send(new RejectSubmissionCommand(id, adminId, feedback), ct);
        return Ok(ApiResponse.Ok("Submission rejected."));
    }
}
