using MediatR;
using Ramendo.Application.Common;
using Ramendo.Application.Submissions.DTOs;
using Ramendo.Application.Submissions.Queries;
using Ramendo.Domain.Aggregates.Submissions;
using Ramendo.Domain.Aggregates.Users;

namespace Ramendo.Application.Submissions.Handlers;

public sealed class GetSubmissionsQueryHandler(IShopSubmissionRepository submissions, IUserRepository users)
    : IRequestHandler<GetSubmissionsQuery, PagedResult<ShopSubmissionDto>>
{
    public async Task<PagedResult<ShopSubmissionDto>> Handle(GetSubmissionsQuery q, CancellationToken ct)
    {
        SubmissionStatus? status = q.Status is null ? null : Enum.Parse<SubmissionStatus>(q.Status);
        var (items, total) = await submissions.GetAllAsync(q.Page, q.Limit, status, ct);

        var userIds = items.Select(s => s.UserId).Distinct();
        var userMap = (await users.GetManyByIdsAsync(userIds, ct))
            .ToDictionary(u => u.Id, u => u.Name);

        var dtos = items.Select(s => ToDto(s, userMap.GetValueOrDefault(s.UserId))).ToList();
        return PagedResult<ShopSubmissionDto>.Create(dtos, total, q.Page, q.Limit);
    }

    internal static ShopSubmissionDto ToDto(ShopSubmission s, string? submittedByName) => new(
        s.Id.ToString(), s.Name, s.City, s.District, s.DetailAddress,
        s.Description, s.Status.ToString(),
        s.UserId.ToString(), submittedByName,
        s.CreatedAt,
        s.Status != SubmissionStatus.Pending ? s.UpdatedAt : (DateTime?)null,
        s.Feedback);
}
