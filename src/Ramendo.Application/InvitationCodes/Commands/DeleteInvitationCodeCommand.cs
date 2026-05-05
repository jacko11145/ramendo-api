using MediatR;

namespace Ramendo.Application.InvitationCodes.Commands;

public sealed record DeleteInvitationCodeCommand(string Code) : IRequest;
