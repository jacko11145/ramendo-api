using MediatR;
using Ramendo.Application.Auth.Commands;
using Ramendo.Application.Auth.DTOs;
using Ramendo.Application.Common;
using Ramendo.Domain.Aggregates.Users;

namespace Ramendo.Application.Auth.Handlers;

public sealed class RefreshTokenCommandHandler(
    IUserRepository users,
    IJwtTokenService jwt) : IRequestHandler<RefreshTokenCommand, AuthResponseDto>
{
    public async Task<AuthResponseDto> Handle(RefreshTokenCommand cmd, CancellationToken ct)
    {
        var userId = await jwt.ValidateRefreshTokenAsync(cmd.RefreshToken, ct)
            ?? throw new UnauthorizedException("Invalid or expired refresh token.");

        var user = await users.GetByIdAsync(userId, ct)
            ?? throw new UnauthorizedException("User not found.");

        if (!user.IsActive)
            throw new ForbiddenException("Account is disabled.");

        await jwt.RevokeRefreshTokenAsync(cmd.RefreshToken, ct);

        var access = jwt.GenerateAccessToken(user);
        var refresh = await jwt.GenerateRefreshTokenAsync(user.Id, ct);

        return new AuthResponseDto(access, refresh, ToSession(user));
    }

    private static UserSessionDto ToSession(User u) => new(
        u.Id.ToString(), u.Email, u.Name, u.Image,
        u.Role.ToString(), u.Experience.Points, u.Experience.Level,
        u.VIP.IsActive, u.VIP.MembershipExpiry);
}
