using MediatR;
using Ramendo.Application.Common;
using Ramendo.Application.Submissions.DTOs;
using Ramendo.Application.Submissions.Queries;
using Ramendo.Domain.Aggregates.Submissions;

namespace Ramendo.Application.Submissions.Handlers;

public sealed class GetSubmissionsQueryHandler(IShopSubmissionRepository submissions)
    : IRequestHandler<GetSubmissionsQuery, PagedResult<ShopSubmissionDto>>
{
    public async Task<PagedResult<ShopSubmissionDto>> Handle(GetSubmissionsQuery q, CancellationToken ct)
    {
        var (items, total) = await submissions.GetAllAsync(q.Page, q.Limit, ct);
        var dtos = items.Select(ToDto).ToList();
        return PagedResult<ShopSubmissionDto>.Create(dtos, total, q.Page, q.Limit);
    }

    internal static ShopSubmissionDto ToDto(ShopSubmission s) => new(
        s.Id.ToString(), s.Name, s.Description, s.City, s.District,
        s.DetailAddress, s.Phone, s.Website, [.. s.Images], [.. s.Types],
        s.Status.ToString(), s.Feedback, s.UserId.ToString(), s.CreatedAt);
}
