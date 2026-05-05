using MediatR;

namespace Ramendo.Application.Shops.Commands;

public sealed record DeleteMenuItemCommand(Guid ShopGuid, Guid MenuItemId) : IRequest;
