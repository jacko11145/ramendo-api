using MediatR;
using Ramendo.Application.Common;
using Ramendo.Application.Users.Commands;
using Ramendo.Domain.Aggregates.Users;

namespace Ramendo.Application.Users.Handlers;

public sealed class UpdateUserRoleCommandHandler(IUserRepository users) : IRequestHandler<UpdateUserRoleCommand>
{
    public async Task Handle(UpdateUserRoleCommand cmd, CancellationToken ct)
    {
        var user = await users.GetByIdAsync(cmd.UserId, ct)
            ?? throw new NotFoundException("User", cmd.UserId);

        if (!Enum.TryParse<UserRole>(cmd.Role, true, out var role))
            throw new ValidationException([$"Invalid role: {cmd.Role}"]);

        user.ChangeRole(role);
        await users.UpdateAsync(user, ct);
    }
}
