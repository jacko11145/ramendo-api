using Ramendo.Domain.Common;

namespace Ramendo.Domain.Aggregates.Shops;

public sealed class ItemOption : Entity
{
    public Guid MenuItemId { get; private set; }
    public Guid OptionTypeId { get; private set; }
    public OptionType OptionType { get; private set; } = null!;
    public bool IsRequired { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    private readonly List<MenuItemOptionValue> _optionValues = [];
    public IReadOnlyList<MenuItemOptionValue> OptionValues => _optionValues.AsReadOnly();

    private ItemOption() { }

    public static ItemOption Create(Guid menuItemId, Guid optionTypeId, bool isRequired) => new()
    {
        Id = Guid.NewGuid(), MenuItemId = menuItemId, OptionTypeId = optionTypeId,
        IsRequired = isRequired, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow
    };

    public void AddOptionValue(MenuItemOptionValue val) => _optionValues.Add(val);
}
