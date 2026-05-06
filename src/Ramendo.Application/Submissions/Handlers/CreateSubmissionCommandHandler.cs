using MediatR;
using Ramendo.Application.Submissions.Commands;
using Ramendo.Domain.Aggregates.Submissions;

namespace Ramendo.Application.Submissions.Handlers;

public sealed class CreateSubmissionCommandHandler(IShopSubmissionRepository submissions)
    : IRequestHandler<CreateSubmissionCommand, string>
{
    public async Task<string> Handle(CreateSubmissionCommand cmd, CancellationToken ct)
    {
        var dto = cmd.Dto;
        var submission = ShopSubmission.Create(
            cmd.UserId, dto.ShopName, dto.Note, dto.City, dto.District,
            dto.Address, null, null, null, [], []);

        await submissions.AddAsync(submission, ct);
        return submission.Id.ToString();
    }
}
