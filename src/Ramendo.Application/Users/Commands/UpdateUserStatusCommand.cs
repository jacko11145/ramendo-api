using MediatR;

namespace Ramendo.Application.Users.Commands;

public sealed record UpdateUserStatusCommand(Guid UserId, bool IsActive) : IRequest;
