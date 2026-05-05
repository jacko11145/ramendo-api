using Ramendo.Domain.Common;

namespace Ramendo.Domain.Aggregates.Shops;

public sealed class OptionValue : Entity
{
    public string Value { get; private set; } = null!;
    public float Price { get; private set; }
    public Guid OptionTypeId { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    private OptionValue() { }

    public static OptionValue Create(string value, float price, Guid optionTypeId) => new()
    {
        Id = Guid.NewGuid(), Value = value, Price = price,
        OptionTypeId = optionTypeId,
        CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow
    };

    public void Update(string value, float price)
    {
        Value = value; Price = price; UpdatedAt = DateTime.UtcNow;
    }
}
