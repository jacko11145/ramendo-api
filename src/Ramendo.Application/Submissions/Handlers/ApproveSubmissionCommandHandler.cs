using MediatR;
using Ramendo.Application.Common;
using Ramendo.Application.Submissions.Commands;
using Ramendo.Domain.Aggregates.Shops;
using Ramendo.Domain.Aggregates.Submissions;
using Ramendo.Domain.Aggregates.Users;

namespace Ramendo.Application.Submissions.Handlers;

public sealed class ApproveSubmissionCommandHandler(
    IShopSubmissionRepository submissions,
    IRamenShopRepository shops,
    IUserRepository users) : IRequestHandler<ApproveSubmissionCommand, string>
{
    public async Task<string> Handle(ApproveSubmissionCommand cmd, CancellationToken ct)
    {
        var sub = await submissions.GetByIdAsync(cmd.SubmissionId, ct)
            ?? throw new NotFoundException("ShopSubmission", cmd.SubmissionId);

        if (sub.Status != SubmissionStatus.Pending)
            throw new ConflictException("Submission is not pending.");

        var shop = RamenShop.Create(sub.Name, sub.Description, sub.City, sub.District,
            sub.DetailAddress, sub.Phone, sub.Website, sub.FacebookPageId, null, [.. sub.Types]);
        shop.SetImages(sub.Images);
        shop.Verify();

        await shops.AddAsync(shop, ct);
        sub.Approve(cmd.ApprovedById, shop.Id);
        await submissions.UpdateAsync(sub, ct);

        var submitter = await users.GetByIdAsync(sub.UserId, ct);
        if (submitter is not null)
        {
            submitter.ApplyExperienceDelta(50);
            await users.UpdateAsync(submitter, ct);
        }

        return shop.Guid.ToString();
    }
}
