using MediatR;

namespace Ramendo.Application.Shops.Commands;

public sealed record AddMenuItemCommand(
    Guid ShopGuid, string Name, string Price, string? Description,
    string Category, string? CustomCategory, bool IsHighlight, bool IsLimited, int Position)
    : IRequest<string>;
