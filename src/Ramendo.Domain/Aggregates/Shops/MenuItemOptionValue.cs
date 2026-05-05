using Ramendo.Domain.Common;

namespace Ramendo.Domain.Aggregates.Shops;

public sealed class MenuItemOptionValue : Entity
{
    public Guid ItemOptionId { get; private set; }
    public Guid OptionValueId { get; private set; }
    public OptionValue OptionValue { get; private set; } = null!;
    public float Price { get; private set; }
    public bool IsDefault { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    private MenuItemOptionValue() { }

    public static MenuItemOptionValue Create(Guid itemOptionId, Guid optionValueId, float price, bool isDefault) => new()
    {
        Id = Guid.NewGuid(), ItemOptionId = itemOptionId, OptionValueId = optionValueId,
        Price = price, IsDefault = isDefault,
        CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow
    };
}
