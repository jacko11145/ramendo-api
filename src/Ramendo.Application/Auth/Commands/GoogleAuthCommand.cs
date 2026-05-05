using MediatR;
using Ramendo.Application.Auth.DTOs;

namespace Ramendo.Application.Auth.Commands;

public sealed record GoogleAuthCommand(string IdToken) : IRequest<AuthResponseDto>;
