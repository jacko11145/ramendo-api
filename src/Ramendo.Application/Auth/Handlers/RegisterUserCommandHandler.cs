using MediatR;
using Ramendo.Application.Auth.Commands;
using Ramendo.Application.Auth.DTOs;
using Ramendo.Application.Common;
using Ramendo.Domain.Aggregates.InvitationCodes;
using Ramendo.Domain.Aggregates.Users;

namespace Ramendo.Application.Auth.Handlers;

public sealed class RegisterUserCommandHandler(
    IUserRepository users,
    IInvitationCodeRepository codes,
    IJwtTokenService jwt,
    IPasswordHasher hasher) : IRequestHandler<RegisterUserCommand, AuthResponseDto>
{
    public async Task<AuthResponseDto> Handle(RegisterUserCommand cmd, CancellationToken ct)
    {
        if (await users.GetByEmailAsync(cmd.Email, ct) is not null)
            throw new ConflictException("Email already registered.");

        if (!string.IsNullOrWhiteSpace(cmd.InvitationCode))
        {
            var code = await codes.GetByCodeAsync(cmd.InvitationCode, ct)
                ?? throw new NotFoundException("InvitationCode", cmd.InvitationCode);

            if (!code.IsValid())
                throw new ForbiddenException("Invitation code is invalid or expired.");

            code.Use();
            await codes.UpdateAsync(code, ct);
        }

        var user = User.Create(cmd.Email, hasher.Hash(cmd.Password), cmd.Name, cmd.InvitationCode);
        await users.AddAsync(user, ct);

        var access = jwt.GenerateAccessToken(user);
        var refresh = await jwt.GenerateRefreshTokenAsync(user.Id, ct);

        return new AuthResponseDto(access, refresh, ToSession(user));
    }

    private static UserSessionDto ToSession(User u) => new(
        u.Id.ToString(), u.Email, u.Name, u.Image,
        u.Role.ToString(), u.Experience.Points, u.Experience.Level,
        u.VIP.IsActive, u.VIP.MembershipExpiry);
}
