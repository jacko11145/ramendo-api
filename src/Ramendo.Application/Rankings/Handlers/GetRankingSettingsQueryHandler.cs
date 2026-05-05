using System.Text.Json;
using MediatR;
using Ramendo.Application.Common;
using Ramendo.Application.Rankings.DTOs;
using Ramendo.Application.Rankings.Queries;

namespace Ramendo.Application.Rankings.Handlers;

public sealed class GetRankingSettingsQueryHandler(ISystemSettingsRepository settings)
    : IRequestHandler<GetRankingSettingsQuery, RankingSettingsDto>
{
    private static readonly RankingSettingsDto Defaults = new(
        true, true, true, "user", 1.0f, 0.8f, 20, 5, 3.5f, true);

    public async Task<RankingSettingsDto> Handle(GetRankingSettingsQuery q, CancellationToken ct)
    {
        var json = await settings.GetAsync("ranking_settings", ct);
        if (json is null) return Defaults;
        try { return JsonSerializer.Deserialize<RankingSettingsDto>(json) ?? Defaults; }
        catch { return Defaults; }
    }
}
