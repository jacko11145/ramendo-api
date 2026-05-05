namespace Ramendo.Application.Common;

public interface ISystemSettingsRepository
{
    Task<string?> GetAsync(string key, CancellationToken ct = default);
    Task SetAsync(string key, string jsonValue, CancellationToken ct = default);
}
