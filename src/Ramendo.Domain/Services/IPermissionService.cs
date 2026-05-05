namespace Ramendo.Domain.Services;

public interface IPermissionService
{
    Task<bool> CanSubmitShopAsync(int userLevel, CancellationToken ct = default);
    Task<bool> CanWriteReviewAsync(int userLevel, CancellationToken ct = default);
    Task<bool> CanRateMenuItemAsync(int userLevel, CancellationToken ct = default);
}
