using MediatR;
using Ramendo.Application.Common;
using Ramendo.Application.Submissions.Commands;
using Ramendo.Domain.Aggregates.Submissions;

namespace Ramendo.Application.Submissions.Handlers;

public sealed class RejectSubmissionCommandHandler(IShopSubmissionRepository submissions) : IRequestHandler<RejectSubmissionCommand>
{
    public async Task Handle(RejectSubmissionCommand cmd, CancellationToken ct)
    {
        var sub = await submissions.GetByIdAsync(cmd.SubmissionId, ct)
            ?? throw new NotFoundException("ShopSubmission", cmd.SubmissionId);

        if (sub.Status != SubmissionStatus.Pending)
            throw new ConflictException("Submission is not pending.");

        sub.Reject(cmd.ApprovedById, cmd.Feedback);
        await submissions.UpdateAsync(sub, ct);
    }
}
