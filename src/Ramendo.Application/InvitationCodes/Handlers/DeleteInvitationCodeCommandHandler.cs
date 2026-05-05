using MediatR;
using Ramendo.Application.Common;
using Ramendo.Application.InvitationCodes.Commands;
using Ramendo.Domain.Aggregates.InvitationCodes;

namespace Ramendo.Application.InvitationCodes.Handlers;

public sealed class DeleteInvitationCodeCommandHandler(IInvitationCodeRepository codes) : IRequestHandler<DeleteInvitationCodeCommand>
{
    public async Task Handle(DeleteInvitationCodeCommand cmd, CancellationToken ct)
    {
        var code = await codes.GetByCodeAsync(cmd.Code, ct)
            ?? throw new NotFoundException("InvitationCode", cmd.Code);

        await codes.DeleteAsync(code.Code, ct);
    }
}
