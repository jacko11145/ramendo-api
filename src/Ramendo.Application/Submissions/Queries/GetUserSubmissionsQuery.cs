using MediatR;
using Ramendo.Application.Submissions.DTOs;

namespace Ramendo.Application.Submissions.Queries;

public sealed record GetUserSubmissionsQuery(Guid UserId) : IRequest<IReadOnlyList<ShopSubmissionDto>>;
