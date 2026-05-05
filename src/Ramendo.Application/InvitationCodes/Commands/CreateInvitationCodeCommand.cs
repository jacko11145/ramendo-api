using MediatR;
using Ramendo.Application.InvitationCodes.DTOs;

namespace Ramendo.Application.InvitationCodes.Commands;

public sealed record CreateInvitationCodeCommand(Guid AdminId, int MaxUses, DateTime? ExpiresAt)
    : IRequest<InvitationCodeDto>;
