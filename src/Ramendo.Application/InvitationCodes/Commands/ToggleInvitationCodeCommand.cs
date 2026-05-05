using MediatR;
using Ramendo.Application.InvitationCodes.DTOs;

namespace Ramendo.Application.InvitationCodes.Commands;

public sealed record ToggleInvitationCodeCommand(string Code) : IRequest<InvitationCodeDto>;
