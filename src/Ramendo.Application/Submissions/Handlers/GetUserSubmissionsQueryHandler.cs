using MediatR;
using Ramendo.Application.Submissions.DTOs;
using Ramendo.Application.Submissions.Queries;
using Ramendo.Domain.Aggregates.Submissions;

namespace Ramendo.Application.Submissions.Handlers;

public sealed class GetUserSubmissionsQueryHandler(IShopSubmissionRepository submissions)
    : IRequestHandler<GetUserSubmissionsQuery, IReadOnlyList<ShopSubmissionDto>>
{
    public async Task<IReadOnlyList<ShopSubmissionDto>> Handle(GetUserSubmissionsQuery q, CancellationToken ct)
    {
        var items = await submissions.GetByUserAsync(q.UserId, ct);
        return items.Select(s => GetSubmissionsQueryHandler.ToDto(s, null)).ToList();
    }
}
