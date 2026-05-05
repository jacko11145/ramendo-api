using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ramendo.Application.Common;
using Ramendo.Application.Submissions.Commands;
using Ramendo.Application.Submissions.DTOs;

namespace Ramendo.Api.Controllers;

[ApiController]
[Route("api/submissions")]
[Authorize]
public sealed class SubmissionsController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<ApiResponse<string>>> Create(
        [FromBody] CreateSubmissionDto dto, CancellationToken ct)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var id = await mediator.Send(new CreateSubmissionCommand(userId, dto), ct);
        return Ok(ApiResponse<string>.Ok(id, "Submission created."));
    }
}
