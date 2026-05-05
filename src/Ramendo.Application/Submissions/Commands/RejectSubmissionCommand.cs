using MediatR;

namespace Ramendo.Application.Submissions.Commands;

public sealed record RejectSubmissionCommand(Guid SubmissionId, Guid ApprovedById, string? Feedback) : IRequest;
