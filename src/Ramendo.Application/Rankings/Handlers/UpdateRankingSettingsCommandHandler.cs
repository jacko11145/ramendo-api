using System.Text.Json;
using MediatR;
using Ramendo.Application.Common;
using Ramendo.Application.Rankings.Commands;
using Ramendo.Application.Rankings.DTOs;

namespace Ramendo.Application.Rankings.Handlers;

public sealed class UpdateRankingSettingsCommandHandler(ISystemSettingsRepository settings)
    : IRequestHandler<UpdateRankingSettingsCommand, RankingSettingsDto>
{
    public async Task<RankingSettingsDto> Handle(UpdateRankingSettingsCommand cmd, CancellationToken ct)
    {
        var json = JsonSerializer.Serialize(cmd.Settings);
        await settings.SetAsync("ranking_settings", json, ct);
        return cmd.Settings;
    }
}
