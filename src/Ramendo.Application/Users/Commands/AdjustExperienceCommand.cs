using MediatR;

namespace Ramendo.Application.Users.Commands;

public sealed record AdjustExperienceCommand(Guid UserId, int Points) : IRequest;
