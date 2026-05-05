using Ramendo.Domain.Aggregates.Shops.Events;
using Ramendo.Domain.Common;

namespace Ramendo.Domain.Aggregates.Shops;

public sealed class RamenShop : AggregateRoot
{
    public Guid Guid { get; private set; }
    public string Name { get; private set; } = null!;
    public string? Description { get; private set; }
    public string City { get; private set; } = null!;
    public string District { get; private set; } = null!;
    public string DetailAddress { get; private set; } = null!;
    public string? Address { get; private set; }
    public string? Phone { get; private set; }
    public string? Website { get; private set; }
    public string? FacebookPageId { get; private set; }
    public List<string> Images { get; private set; } = [];
    public string? CoverImage { get; private set; }
    public float Rating { get; private set; }
    public float GoogleRating { get; private set; }
    public float CriticRating { get; private set; }
    public int ReviewCount { get; private set; }
    public int GoogleReviewCount { get; private set; }
    public int CriticReviewCount { get; private set; }
    public List<string> Types { get; private set; } = [];
    public bool IsActive { get; private set; }
    public bool IsVerified { get; private set; }
    public BusinessHours? BusinessHours { get; private set; }
    public List<NewsItem> NewsItems { get; private set; } = [];
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    private readonly List<MenuItem> _menuItems = [];
    public IReadOnlyList<MenuItem> MenuItems => _menuItems.AsReadOnly();

    private RamenShop() { }

    public static RamenShop Create(string name, string? description, string city, string district,
        string detailAddress, string? phone, string? website, string? facebookPageId, string[] types)
    {
        var shop = new RamenShop
        {
            Id = Guid.NewGuid(), Guid = Guid.NewGuid(),
            Name = name, Description = description,
            City = city, District = district, DetailAddress = detailAddress,
            Phone = phone, Website = website, FacebookPageId = facebookPageId,
            Types = [.. types], IsActive = true, IsVerified = false,
            CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow
        };
        shop.RaiseDomainEvent(new ShopCreatedEvent(shop.Id, name));
        return shop;
    }

    public void Update(string name, string? description, string city, string district,
        string detailAddress, string? phone, string? website, string? facebookPageId,
        string[] types, bool isActive)
    {
        Name = name; Description = description; City = city; District = district;
        DetailAddress = detailAddress; Phone = phone; Website = website;
        FacebookPageId = facebookPageId; Types = [.. types]; IsActive = isActive;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetCoverImage(string? url) { CoverImage = url; UpdatedAt = DateTime.UtcNow; }
    public void SetImages(IEnumerable<string> images) { Images = [.. images]; UpdatedAt = DateTime.UtcNow; }
    public void SetBusinessHours(BusinessHours? hours) { BusinessHours = hours; UpdatedAt = DateTime.UtcNow; }
    public void SetNewsItems(IEnumerable<NewsItem> items) { NewsItems = [.. items]; UpdatedAt = DateTime.UtcNow; }

    public void Verify()
    {
        IsVerified = true; UpdatedAt = DateTime.UtcNow;
        RaiseDomainEvent(new ShopVerifiedEvent(Id));
    }

    public void UpdateRating(float rating, int reviewCount)
    {
        Rating = rating; ReviewCount = reviewCount; UpdatedAt = DateTime.UtcNow;
    }

    public MenuItem AddMenuItem(string name, string price, string? description, string category,
        string? customCategory, bool isHighlight, bool isLimited, int position)
    {
        var item = MenuItem.Create(Id, name, price, description, category, customCategory, isHighlight, isLimited, position);
        _menuItems.Add(item);
        UpdatedAt = DateTime.UtcNow;
        RaiseDomainEvent(new MenuItemAddedEvent(Id, item.Id));
        return item;
    }

    public void RemoveMenuItem(Guid menuItemId)
    {
        _menuItems.RemoveAll(m => m.Id == menuItemId);
        UpdatedAt = DateTime.UtcNow;
    }

    public void ReorderMenuItems(IEnumerable<(Guid Id, int Position)> order)
    {
        foreach (var (id, pos) in order)
            _menuItems.FirstOrDefault(m => m.Id == id)?.SetPosition(pos);
        UpdatedAt = DateTime.UtcNow;
    }
}
