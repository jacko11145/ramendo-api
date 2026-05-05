using MediatR;
using Ramendo.Application.InvitationCodes.DTOs;
using Ramendo.Application.InvitationCodes.Queries;
using Ramendo.Domain.Aggregates.InvitationCodes;

namespace Ramendo.Application.InvitationCodes.Handlers;

public sealed class GetInvitationCodesQueryHandler(IInvitationCodeRepository codes)
    : IRequestHandler<GetInvitationCodesQuery, IReadOnlyList<InvitationCodeDto>>
{
    public async Task<IReadOnlyList<InvitationCodeDto>> Handle(GetInvitationCodesQuery q, CancellationToken ct)
    {
        var all = await codes.GetAllAsync(ct);
        return all.Select(c => new InvitationCodeDto(
            c.Code, c.CreatedById.ToString(), c.IsActive,
            c.MaxUses, c.UsedCount, c.ExpiresAt, c.CreatedAt)).ToList();
    }
}
