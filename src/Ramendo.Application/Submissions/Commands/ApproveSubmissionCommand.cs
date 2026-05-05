using MediatR;

namespace Ramendo.Application.Submissions.Commands;

public sealed record ApproveSubmissionCommand(Guid SubmissionId, Guid ApprovedById) : IRequest<string>;
