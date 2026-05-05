using MediatR;
using Ramendo.Application.Common;
using Ramendo.Application.Users.Commands;
using Ramendo.Domain.Aggregates.Users;

namespace Ramendo.Application.Users.Handlers;

public sealed class DeleteUserCommandHandler(IUserRepository users) : IRequestHandler<DeleteUserCommand>
{
    public async Task Handle(DeleteUserCommand cmd, CancellationToken ct)
    {
        var user = await users.GetByIdAsync(cmd.UserId, ct)
            ?? throw new NotFoundException("User", cmd.UserId);
        await users.DeleteAsync(user.Id, ct);
    }
}
