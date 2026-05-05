using System.Security.Cryptography;
using MediatR;
using Ramendo.Application.InvitationCodes.Commands;
using Ramendo.Application.InvitationCodes.DTOs;
using Ramendo.Domain.Aggregates.InvitationCodes;

namespace Ramendo.Application.InvitationCodes.Handlers;

public sealed class CreateInvitationCodeCommandHandler(IInvitationCodeRepository codes)
    : IRequestHandler<CreateInvitationCodeCommand, InvitationCodeDto>
{
    public async Task<InvitationCodeDto> Handle(CreateInvitationCodeCommand cmd, CancellationToken ct)
    {
        var code = Convert.ToHexString(RandomNumberGenerator.GetBytes(6)).ToUpperInvariant();
        var invCode = InvitationCode.Create(code, cmd.AdminId, cmd.MaxUses, cmd.ExpiresAt);
        await codes.AddAsync(invCode, ct);

        return new InvitationCodeDto(
            invCode.Code, invCode.CreatedById.ToString(), invCode.IsActive,
            invCode.MaxUses, invCode.UsedCount, invCode.ExpiresAt, invCode.CreatedAt);
    }
}
