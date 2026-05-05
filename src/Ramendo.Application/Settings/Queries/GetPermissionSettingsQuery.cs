using MediatR;
using Ramendo.Application.Settings.DTOs;

namespace Ramendo.Application.Settings.Queries;

public sealed record GetPermissionSettingsQuery : IRequest<PermissionSettingsDto>;
