using MediatR;
using Ramendo.Application.Auth.Commands;
using Ramendo.Application.Auth.DTOs;
using Ramendo.Application.Common;
using Ramendo.Domain.Aggregates.Users;

namespace Ramendo.Application.Auth.Handlers;

public sealed class LoginCommandHandler(
    IUserRepository users,
    IJwtTokenService jwt,
    IPasswordHasher hasher) : IRequestHandler<LoginCommand, AuthResponseDto>
{
    public async Task<AuthResponseDto> Handle(LoginCommand cmd, CancellationToken ct)
    {
        var user = await users.GetByEmailAsync(cmd.Email, ct)
            ?? throw new UnauthorizedException("Invalid email or password.");

        if (user.PasswordHash is null || !hasher.Verify(cmd.Password, user.PasswordHash))
            throw new UnauthorizedException("Invalid email or password.");

        if (!user.IsActive)
            throw new ForbiddenException("Account is disabled.");

        var access = jwt.GenerateAccessToken(user);
        var refresh = await jwt.GenerateRefreshTokenAsync(user.Id, ct);

        return new AuthResponseDto(access, refresh, ToSession(user));
    }

    private static UserSessionDto ToSession(User u) => new(
        u.Id.ToString(), u.Email, u.Name, u.Image,
        u.Role.ToString(), u.Experience.Points, u.Experience.Level,
        u.VIP.IsActive, u.VIP.MembershipExpiry);
}
