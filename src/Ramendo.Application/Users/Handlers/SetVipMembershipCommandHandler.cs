using MediatR;
using Ramendo.Application.Common;
using Ramendo.Application.Users.Commands;
using Ramendo.Domain.Aggregates.Users;

namespace Ramendo.Application.Users.Handlers;

public sealed class SetVipMembershipCommandHandler(IUserRepository users) : IRequestHandler<SetVipMembershipCommand>
{
    public async Task Handle(SetVipMembershipCommand cmd, CancellationToken ct)
    {
        var user = await users.GetByIdAsync(cmd.UserId, ct)
            ?? throw new NotFoundException("User", cmd.UserId);

        user.SetVIP(cmd.IsVIP, cmd.MembershipExpiry);
        await users.UpdateAsync(user, ct);
    }
}
