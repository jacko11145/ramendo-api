using System.Text.Json;
using MediatR;
using Ramendo.Application.Common;
using Ramendo.Application.Settings.Commands;
using Ramendo.Application.Settings.DTOs;

namespace Ramendo.Application.Settings.Handlers;

public sealed class UpdatePermissionSettingsCommandHandler(ISystemSettingsRepository settings)
    : IRequestHandler<UpdatePermissionSettingsCommand, PermissionSettingsDto>
{
    public async Task<PermissionSettingsDto> Handle(UpdatePermissionSettingsCommand cmd, CancellationToken ct)
    {
        var json = JsonSerializer.Serialize(cmd.Settings);
        await settings.SetAsync("permission_settings", json, ct);
        return cmd.Settings;
    }
}
