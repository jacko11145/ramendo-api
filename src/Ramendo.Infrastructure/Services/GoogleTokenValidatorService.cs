using Google.Apis.Auth;
using Microsoft.Extensions.Configuration;
using Ramendo.Application.Common;

namespace Ramendo.Infrastructure.Services;

public sealed class GoogleTokenValidatorService(IConfiguration config) : IGoogleTokenValidator
{
    private readonly string _clientId = config["Google:ClientId"] ?? throw new InvalidOperationException("Google:ClientId not configured");

    public async Task<GoogleUserInfo?> ValidateAsync(string idToken, CancellationToken ct = default)
    {
        try
        {
            var payload = await GoogleJsonWebSignature.ValidateAsync(idToken,
                new GoogleJsonWebSignature.ValidationSettings { Audience = [_clientId] });

            return new GoogleUserInfo(payload.Email, payload.Name, payload.Picture);
        }
        catch
        {
            return null;
        }
    }
}
