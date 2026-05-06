using MediatR;
using Ramendo.Application.Common;
using Ramendo.Application.Submissions.Commands;
using Ramendo.Domain.Aggregates.Submissions;
using Ramendo.Domain.Aggregates.Users;
using Ramendo.Domain.Services;

namespace Ramendo.Application.Submissions.Handlers;

public sealed class CreateSubmissionCommandHandler(
    IShopSubmissionRepository submissions,
    IUserRepository users,
    IPermissionService permissions) : IRequestHandler<CreateSubmissionCommand, string>
{
    public async Task<string> Handle(CreateSubmissionCommand cmd, CancellationToken ct)
    {
        var user = await users.GetByIdAsync(cmd.UserId, ct)
            ?? throw new NotFoundException("User", cmd.UserId);

        if (user.Role == UserRole.User && !await permissions.CanSubmitShopAsync(user.Experience.Level, ct))
            throw new ForbiddenException("您的等級不足以提案店家。");

        var dto = cmd.Dto;
        var submission = ShopSubmission.Create(
            cmd.UserId, dto.ShopName, dto.Note, dto.City, dto.District,
            dto.Address, null, null, null, [], []);

        await submissions.AddAsync(submission, ct);
        return submission.Id.ToString();
    }
}
