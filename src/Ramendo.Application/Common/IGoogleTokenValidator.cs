namespace Ramendo.Application.Common;

public sealed record GoogleUserInfo(string Email, string? Name, string? Picture);

public interface IGoogleTokenValidator
{
    Task<GoogleUserInfo?> ValidateAsync(string idToken, CancellationToken ct = default);
}
