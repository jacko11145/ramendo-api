using MediatR;
using Ramendo.Application.Common;
using Ramendo.Application.Users.Commands;
using Ramendo.Domain.Aggregates.Users;

namespace Ramendo.Application.Users.Handlers;

public sealed class AdjustExperienceCommandHandler(IUserRepository users) : IRequestHandler<AdjustExperienceCommand>
{
    public async Task Handle(AdjustExperienceCommand cmd, CancellationToken ct)
    {
        var user = await users.GetByIdAsync(cmd.UserId, ct)
            ?? throw new NotFoundException("User", cmd.UserId);

        user.AdjustExperience(cmd.Points);
        await users.UpdateAsync(user, ct);
    }
}
