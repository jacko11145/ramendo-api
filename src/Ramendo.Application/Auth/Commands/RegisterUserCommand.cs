using MediatR;
using Ramendo.Application.Auth.DTOs;

namespace Ramendo.Application.Auth.Commands;

public sealed record RegisterUserCommand(string Email, string Password, string? Name, string InvitationCode)
    : IRequest<AuthResponseDto>;
