using Ramendo.Domain.Common;

namespace Ramendo.Domain.Aggregates.Shops;

public sealed class OptionType : Entity
{
    public string Name { get; private set; } = null!;
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    private readonly List<OptionValue> _optionValues = [];
    public IReadOnlyList<OptionValue> OptionValues => _optionValues.AsReadOnly();

    private OptionType() { }

    public static OptionType Create(string name) => new()
    {
        Id = Guid.NewGuid(), Name = name,
        CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow
    };

    public void AddValue(OptionValue value) => _optionValues.Add(value);
    public void RemoveValue(Guid valueId) => _optionValues.RemoveAll(v => v.Id == valueId);
}
