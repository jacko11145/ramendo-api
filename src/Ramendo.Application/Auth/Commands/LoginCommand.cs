using MediatR;
using Ramendo.Application.Auth.DTOs;

namespace Ramendo.Application.Auth.Commands;

public sealed record LoginCommand(string Email, string Password) : IRequest<AuthResponseDto>;
