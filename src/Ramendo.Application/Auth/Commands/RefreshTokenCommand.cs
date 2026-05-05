using MediatR;
using Ramendo.Application.Auth.DTOs;

namespace Ramendo.Application.Auth.Commands;

public sealed record RefreshTokenCommand(string RefreshToken) : IRequest<AuthResponseDto>;
