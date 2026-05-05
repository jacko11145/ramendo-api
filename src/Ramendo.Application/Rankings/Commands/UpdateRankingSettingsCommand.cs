using MediatR;
using Ramendo.Application.Rankings.DTOs;

namespace Ramendo.Application.Rankings.Commands;

public sealed record UpdateRankingSettingsCommand(RankingSettingsDto Settings) : IRequest<RankingSettingsDto>;
