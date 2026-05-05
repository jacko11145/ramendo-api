using System.Text.Json;
using MediatR;
using Ramendo.Application.Common;
using Ramendo.Application.Settings.DTOs;
using Ramendo.Application.Settings.Queries;

namespace Ramendo.Application.Settings.Handlers;

public sealed class GetPermissionSettingsQueryHandler(ISystemSettingsRepository settings)
    : IRequestHandler<GetPermissionSettingsQuery, PermissionSettingsDto>
{
    private static readonly PermissionSettingsDto Defaults = new(3, 2, 1);

    public async Task<PermissionSettingsDto> Handle(GetPermissionSettingsQuery q, CancellationToken ct)
    {
        var json = await settings.GetAsync("permission_settings", ct);
        if (json is null) return Defaults;
        try { return JsonSerializer.Deserialize<PermissionSettingsDto>(json) ?? Defaults; }
        catch { return Defaults; }
    }
}
