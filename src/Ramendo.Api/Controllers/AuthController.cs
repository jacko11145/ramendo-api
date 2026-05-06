using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ramendo.Application.Auth.Commands;
using Ramendo.Application.Auth.DTOs;
using Ramendo.Application.Common;
using Ramendo.Domain.Aggregates.Users;

namespace Ramendo.Api.Controllers;

[ApiController]
[Route("api/auth")]
public sealed class AuthController(IMediator mediator, IUserRepository users) : ControllerBase
{
    [HttpPost("register")]
    public async Task<ActionResult<ApiResponse<AuthResponseDto>>> Register(
        [FromBody] RegisterRequestDto dto, CancellationToken ct)
    {
        var result = await mediator.Send(new RegisterUserCommand(dto.Email, dto.Password, dto.Name, dto.InvitationCode), ct);
        return Ok(ApiResponse<AuthResponseDto>.Ok(result));
    }

    [HttpPost("login")]
    public async Task<ActionResult<ApiResponse<AuthResponseDto>>> Login(
        [FromBody] LoginRequestDto dto, CancellationToken ct)
    {
        var result = await mediator.Send(new LoginCommand(dto.Email, dto.Password), ct);
        return Ok(ApiResponse<AuthResponseDto>.Ok(result));
    }

    [HttpPost("google")]
    public async Task<ActionResult<ApiResponse<AuthResponseDto>>> Google(
        [FromBody] GoogleAuthRequestDto dto, CancellationToken ct)
    {
        var result = await mediator.Send(new GoogleAuthCommand(dto.IdToken), ct);
        return Ok(ApiResponse<AuthResponseDto>.Ok(result));
    }

    [HttpPost("refresh")]
    public async Task<ActionResult<ApiResponse<AuthResponseDto>>> Refresh(
        [FromBody] RefreshTokenRequestDto dto, CancellationToken ct)
    {
        var result = await mediator.Send(new RefreshTokenCommand(dto.RefreshToken), ct);
        return Ok(ApiResponse<AuthResponseDto>.Ok(result));
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<ActionResult<ApiResponse<UserSessionDto>>> Me(CancellationToken ct)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var user = await users.GetByIdAsync(userId, ct);
        if (user is null) return Unauthorized(ApiResponse.Fail("User not found."));
        return Ok(ApiResponse<UserSessionDto>.Ok(new UserSessionDto(
            user.Id.ToString(), user.Email, user.Name, user.Image,
            user.Role.ToString(), user.Experience.Points, user.Experience.Level,
            user.VIP.IsActive, user.VIP.MembershipExpiry)));
    }

    [HttpPost("logout")]
    [Authorize]
    public IActionResult Logout() => Ok(ApiResponse.Ok("Logged out."));
}
