using Ramendo.Domain.Common;

namespace Ramendo.Domain.Aggregates.Shops;

public sealed class MenuItem : Entity
{
    public string Name { get; private set; } = null!;
    public string Price { get; private set; } = null!;
    public string? Description { get; private set; }
    public string Category { get; private set; } = "主食";
    public string? CustomCategory { get; private set; }
    public string? Image { get; private set; }
    public bool IsHighlight { get; private set; }
    public bool IsLimited { get; private set; }
    public string Status { get; private set; } = "active";
    public int Position { get; private set; }
    public Guid ShopId { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    private readonly List<ItemOption> _itemOptions = [];
    public IReadOnlyList<ItemOption> ItemOptions => _itemOptions.AsReadOnly();

    private MenuItem() { }

    public static MenuItem Create(Guid shopId, string name, string price, string? description,
        string category, string? customCategory, bool isHighlight, bool isLimited, int position) => new()
    {
        Id = Guid.NewGuid(), ShopId = shopId, Name = name, Price = price,
        Description = description, Category = category, CustomCategory = customCategory,
        IsHighlight = isHighlight, IsLimited = isLimited, Position = position,
        Status = "active", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow
    };

    public void Update(string name, string price, string? description, string category,
        string? customCategory, string? image, bool isHighlight, bool isLimited, string status)
    {
        Name = name; Price = price; Description = description; Category = category;
        CustomCategory = customCategory; Image = image; IsHighlight = isHighlight;
        IsLimited = isLimited; Status = status; UpdatedAt = DateTime.UtcNow;
    }

    public void SetPosition(int pos) { Position = pos; UpdatedAt = DateTime.UtcNow; }
    public void SetImage(string? url) { Image = url; UpdatedAt = DateTime.UtcNow; }
    public void AddItemOption(ItemOption opt) => _itemOptions.Add(opt);
}
