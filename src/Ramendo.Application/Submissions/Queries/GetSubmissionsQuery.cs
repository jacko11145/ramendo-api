using MediatR;
using Ramendo.Application.Common;
using Ramendo.Application.Submissions.DTOs;

namespace Ramendo.Application.Submissions.Queries;

public sealed record GetSubmissionsQuery(int Page = 1, int Limit = 20) : IRequest<PagedResult<ShopSubmissionDto>>;
