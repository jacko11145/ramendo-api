using MediatR;
using Ramendo.Application.Settings.DTOs;

namespace Ramendo.Application.Settings.Commands;

public sealed record UpdatePermissionSettingsCommand(PermissionSettingsDto Settings) : IRequest<PermissionSettingsDto>;
