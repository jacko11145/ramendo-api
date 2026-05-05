using MediatR;
using Ramendo.Application.Submissions.DTOs;

namespace Ramendo.Application.Submissions.Commands;

public sealed record CreateSubmissionCommand(Guid UserId, CreateSubmissionDto Dto) : IRequest<string>;
