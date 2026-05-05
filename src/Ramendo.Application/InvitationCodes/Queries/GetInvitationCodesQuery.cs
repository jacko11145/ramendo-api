using MediatR;
using Ramendo.Application.InvitationCodes.DTOs;

namespace Ramendo.Application.InvitationCodes.Queries;

public sealed record GetInvitationCodesQuery : IRequest<IReadOnlyList<InvitationCodeDto>>;
