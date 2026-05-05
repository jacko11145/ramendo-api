using MediatR;

namespace Ramendo.Application.Users.Commands;

public sealed record UpdateUserRoleCommand(Guid UserId, string Role) : IRequest;
