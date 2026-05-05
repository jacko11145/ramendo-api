using MediatR;
using Ramendo.Application.Settings.DTOs;

namespace Ramendo.Application.Settings.Queries;

public sealed record GetDashboardStatsQuery : IRequest<DashboardStatsDto>;
