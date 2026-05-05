using MediatR;
using Ramendo.Application.Rankings.DTOs;

namespace Ramendo.Application.Rankings.Queries;

public sealed record GetRankingSettingsQuery : IRequest<RankingSettingsDto>;
