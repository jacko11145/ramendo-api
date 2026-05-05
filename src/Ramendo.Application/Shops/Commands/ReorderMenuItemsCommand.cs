using MediatR;

namespace Ramendo.Application.Shops.Commands;

public sealed record ReorderItem(Guid Id, int Position);

public sealed record ReorderMenuItemsCommand(Guid ShopGuid, IReadOnlyList<ReorderItem> Order) : IRequest;
