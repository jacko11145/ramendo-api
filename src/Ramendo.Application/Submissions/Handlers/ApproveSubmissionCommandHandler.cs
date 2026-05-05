using MediatR;
using Ramendo.Application.Common;
using Ramendo.Application.Submissions.Commands;
using Ramendo.Domain.Aggregates.Shops;
using Ramendo.Domain.Aggregates.Submissions;

namespace Ramendo.Application.Submissions.Handlers;

public sealed class ApproveSubmissionCommandHandler(
    IShopSubmissionRepository submissions,
    IRamenShopRepository shops) : IRequestHandler<ApproveSubmissionCommand, string>
{
    public async Task<string> Handle(ApproveSubmissionCommand cmd, CancellationToken ct)
    {
        var sub = await submissions.GetByIdAsync(cmd.SubmissionId, ct)
            ?? throw new NotFoundException("ShopSubmission", cmd.SubmissionId);

        if (sub.Status != SubmissionStatus.Pending)
            throw new ConflictException("Submission is not pending.");

        // Create the shop from submission data
        var shop = RamenShop.Create(sub.Name, sub.Description, sub.City, sub.District,
            sub.DetailAddress, sub.Phone, sub.Website, sub.FacebookPageId, null, [.. sub.Types]);
        shop.SetImages(sub.Images);
        shop.Verify();

        await shops.AddAsync(shop, ct);
        sub.Approve(cmd.ApprovedById, shop.Id);
        await submissions.UpdateAsync(sub, ct);

        return shop.Guid.ToString();
    }
}
