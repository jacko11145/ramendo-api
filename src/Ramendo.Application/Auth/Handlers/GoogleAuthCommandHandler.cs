using MediatR;
using Ramendo.Application.Auth.Commands;
using Ramendo.Application.Auth.DTOs;
using Ramendo.Application.Common;
using Ramendo.Domain.Aggregates.Users;

namespace Ramendo.Application.Auth.Handlers;

public sealed class GoogleAuthCommandHandler(
    IUserRepository users,
    IJwtTokenService jwt,
    IGoogleTokenValidator googleValidator) : IRequestHandler<GoogleAuthCommand, AuthResponseDto>
{
    public async Task<AuthResponseDto> Handle(GoogleAuthCommand cmd, CancellationToken ct)
    {
        var payload = await googleValidator.ValidateAsync(cmd.IdToken, ct)
            ?? throw new UnauthorizedException("Invalid Google token.");

        var user = await users.GetByEmailAsync(payload.Email, ct);

        if (user is null)
        {
            user = User.Create(payload.Email, null, payload.Name, null);
            user.SetImage(payload.Picture);
            await users.AddAsync(user, ct);
        }
        else if (!user.IsActive)
        {
            throw new ForbiddenException("Account is disabled.");
        }

        var access = jwt.GenerateAccessToken(user);
        var refresh = await jwt.GenerateRefreshTokenAsync(user.Id, ct);

        return new AuthResponseDto(access, refresh, ToSession(user));
    }

    private static UserSessionDto ToSession(User u) => new(
        u.Id.ToString(), u.Email, u.Name, u.Image,
        u.Role.ToString(), u.Experience.Points, u.Experience.Level,
        u.VIP.IsActive, u.VIP.MembershipExpiry);
}
