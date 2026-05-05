using Ramendo.Domain.Common;

namespace Ramendo.Domain.Aggregates.Users;

public sealed class Favorite : Entity
{
    public Guid UserId { get; private set; }
    public Guid RamenShopId { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private Favorite() { }

    public static Favorite Create(Guid userId, Guid shopId) => new()
    {
        Id = Guid.NewGuid(), UserId = userId, RamenShopId = shopId,
        CreatedAt = DateTime.UtcNow
    };
}
