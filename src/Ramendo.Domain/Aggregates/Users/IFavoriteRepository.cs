namespace Ramendo.Domain.Aggregates.Users;

public interface IFavoriteRepository
{
    Task<Favorite?> GetAsync(Guid userId, Guid shopId, CancellationToken ct = default);
    Task<IReadOnlyList<Favorite>> GetByUserAsync(Guid userId, CancellationToken ct = default);
    Task AddAsync(Favorite favorite, CancellationToken ct = default);
    Task DeleteAsync(Guid userId, Guid shopId, CancellationToken ct = default);
}
