using MediatR;
using Ramendo.Application.Rankings.DTOs;

namespace Ramendo.Application.Rankings.Queries;

public sealed record GetRankingsQuery(string Type = "user") : IRequest<IReadOnlyList<RankingItemDto>>;
