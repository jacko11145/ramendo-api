namespace Ramendo.Application.Auth.DTOs;

public sealed record RegisterRequestDto(string Email, string Password, string? Name, string InvitationCode);

public sealed record LoginRequestDto(string Email, string Password);

public sealed record GoogleAuthRequestDto(string IdToken);

public sealed record RefreshTokenRequestDto(string RefreshToken);

public sealed record AuthResponseDto(string AccessToken, string RefreshToken, UserSessionDto User);

public sealed record UserSessionDto(
    string Id, string Email, string? Name, string? Image,
    string Role, int ExperiencePoints, int Level, bool IsVIP, DateTime? MembershipExpiry);
