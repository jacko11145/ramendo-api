namespace Ramendo.Domain.Services;

public interface IPermissionService
{
    Task<bool> CanSubmitShopAsync(int userLevel, CancellationToken ct = default);
    Task<bool> CanReviewAsync(int userLevel, CancellationToken ct = default);
    Task<bool> CanFavoriteAsync(int userLevel, CancellationToken ct = default);
}
