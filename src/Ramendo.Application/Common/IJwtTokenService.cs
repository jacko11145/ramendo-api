using Ramendo.Domain.Aggregates.Users;

namespace Ramendo.Application.Common;

public interface IJwtTokenService
{
    string GenerateAccessToken(User user);
    Task<string> GenerateRefreshTokenAsync(Guid userId, CancellationToken ct = default);
    Task<Guid?> ValidateRefreshTokenAsync(string token, CancellationToken ct = default);
    Task RevokeRefreshTokenAsync(string token, CancellationToken ct = default);
}
