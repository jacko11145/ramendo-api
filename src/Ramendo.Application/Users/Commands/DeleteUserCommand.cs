using MediatR;

namespace Ramendo.Application.Users.Commands;

public sealed record DeleteUserCommand(Guid UserId) : IRequest;
