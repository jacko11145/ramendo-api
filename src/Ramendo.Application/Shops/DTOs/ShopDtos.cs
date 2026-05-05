namespace Ramendo.Application.Shops.DTOs;

public sealed record RamenShopListDto(
    string Id, string Name, string City, string District, string DetailAddress,
    string? CoverImage, float Rating, float GoogleRating, int ReviewCount,
    string[] Types, bool IsActive, bool IsVerified, string? Phone, string? Instagram);

public sealed record RamenShopDetailDto(
    string Id, string Name, string? Description, string City, string District,
    string DetailAddress, string? Phone, string? Website, string? FacebookPageId, string? Instagram,
    string[] Images, string? CoverImage, float Rating, float GoogleRating, float CriticRating,
    int ReviewCount, string[] Types, bool IsActive, bool IsVerified,
    BusinessHoursDto? BusinessHours, NewsItemDto[] NewsItems, MenuItemDto[] MenuItems,
    DateTime CreatedAt);

public sealed record BusinessHoursDto(
    DayHoursDto? Monday, DayHoursDto? Tuesday, DayHoursDto? Wednesday,
    DayHoursDto? Thursday, DayHoursDto? Friday, DayHoursDto? Saturday, DayHoursDto? Sunday);

public sealed record DayHoursDto(
    bool IsOpen, bool IsSplit, string? Open, string? Close,
    string? LunchOpen, string? LunchClose, string? DinnerOpen, string? DinnerClose);

public sealed record NewsItemDto(string Title, string Content, DateTime? StartDate, DateTime? EndDate, DateTime CreatedAt);

public sealed record MenuItemDto(
    string Id, string Name, string Price, string? Description, string Category,
    string? CustomCategory, string? Image, bool IsHighlight, bool IsLimited,
    string Status, int Position, MenuItemOptionGroupDto[] OptionGroups);

public sealed record MenuItemOptionGroupDto(
    string OptionTypeId, string OptionTypeName, bool IsRequired,
    MenuItemOptionValueDto[] Values);

public sealed record MenuItemOptionValueDto(
    string OptionValueId, string Value, float Price, bool IsDefault);

public sealed record CreateUpdateShopDto(
    string Name, string? Description, string City, string District,
    string DetailAddress, string? Phone, string? Website, string? FacebookPageId, string? Instagram,
    string[] Types, bool IsActive, bool IsVerified, float? GoogleRating,
    BusinessHoursDto? BusinessHours, NewsItemDto[]? NewsItems);

public sealed record AddMenuItemRequest(
    string Name, string Price, string? Description, string Category,
    string? CustomCategory, bool IsHighlight, bool IsLimited, int Position);

public sealed record ReorderItemRequest(Guid Id, int Position);
public sealed record ReorderMenuItemsRequest(List<ReorderItemRequest> Order);
