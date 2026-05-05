using MediatR;
using Ramendo.Application.Common;
using Ramendo.Application.InvitationCodes.Commands;
using Ramendo.Application.InvitationCodes.DTOs;
using Ramendo.Domain.Aggregates.InvitationCodes;

namespace Ramendo.Application.InvitationCodes.Handlers;

public sealed class ToggleInvitationCodeCommandHandler(IInvitationCodeRepository codes)
    : IRequestHandler<ToggleInvitationCodeCommand, InvitationCodeDto>
{
    public async Task<InvitationCodeDto> Handle(ToggleInvitationCodeCommand cmd, CancellationToken ct)
    {
        var code = await codes.GetByCodeAsync(cmd.Code, ct)
            ?? throw new NotFoundException("InvitationCode", cmd.Code);

        code.Toggle();
        await codes.UpdateAsync(code, ct);

        return new InvitationCodeDto(
            code.Code, code.CreatedById.ToString(), code.IsActive,
            code.MaxUses, code.UsedCount, code.ExpiresAt, code.CreatedAt);
    }
}
