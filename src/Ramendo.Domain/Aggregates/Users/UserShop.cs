using Ramendo.Domain.Common;

namespace Ramendo.Domain.Aggregates.Users;

public sealed class UserShop : Entity
{
    public Guid UserId { get; private set; }
    public Guid ShopId { get; private set; }
    public string Role { get; private set; } = "owner";
    public DateTime CreatedAt { get; private set; }

    private UserShop() { }

    public static UserShop Create(Guid userId, Guid shopId, string role = "owner") => new()
    {
        Id = Guid.NewGuid(), UserId = userId, ShopId = shopId, Role = role,
        CreatedAt = DateTime.UtcNow
    };
}
