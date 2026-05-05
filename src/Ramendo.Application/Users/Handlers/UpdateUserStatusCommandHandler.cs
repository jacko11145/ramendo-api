using MediatR;
using Ramendo.Application.Common;
using Ramendo.Application.Users.Commands;
using Ramendo.Domain.Aggregates.Users;

namespace Ramendo.Application.Users.Handlers;

public sealed class UpdateUserStatusCommandHandler(IUserRepository users) : IRequestHandler<UpdateUserStatusCommand>
{
    public async Task Handle(UpdateUserStatusCommand cmd, CancellationToken ct)
    {
        var user = await users.GetByIdAsync(cmd.UserId, ct)
            ?? throw new NotFoundException("User", cmd.UserId);

        user.SetActive(cmd.IsActive);
        await users.UpdateAsync(user, ct);
    }
}
