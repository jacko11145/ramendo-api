using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ramendo.Application.Common;
using Ramendo.Application.InvitationCodes.Commands;
using Ramendo.Application.InvitationCodes.DTOs;
using Ramendo.Application.InvitationCodes.Queries;

namespace Ramendo.Api.Controllers.Admin;

[ApiController]
[Route("api/admin/invitation-codes")]
[Authorize(Roles = "Admin")]
public sealed class AdminInvitationCodesController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<InvitationCodeDto>>>> GetCodes(CancellationToken ct)
    {
        var result = await mediator.Send(new GetInvitationCodesQuery(), ct);
        return Ok(ApiResponse<IReadOnlyList<InvitationCodeDto>>.Ok(result));
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<InvitationCodeDto>>> CreateCode(
        [FromBody] CreateInvitationCodeDto dto, CancellationToken ct)
    {
        var adminId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result = await mediator.Send(new CreateInvitationCodeCommand(adminId, dto.MaxUses, dto.ExpiresAt), ct);
        return Ok(ApiResponse<InvitationCodeDto>.Ok(result));
    }

    [HttpPatch("{code}/toggle")]
    public async Task<ActionResult<ApiResponse<InvitationCodeDto>>> Toggle(string code, CancellationToken ct)
    {
        var result = await mediator.Send(new ToggleInvitationCodeCommand(code), ct);
        return Ok(ApiResponse<InvitationCodeDto>.Ok(result));
    }

    [HttpDelete("{code}")]
    public async Task<ActionResult<ApiResponse>> Delete(string code, CancellationToken ct)
    {
        await mediator.Send(new DeleteInvitationCodeCommand(code), ct);
        return Ok(ApiResponse.Ok());
    }
}
