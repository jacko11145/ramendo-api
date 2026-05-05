using Ramendo.Domain.Common;

namespace Ramendo.Domain.Aggregates.Shops.Events;

public sealed record MenuItemAddedEvent(Guid ShopId, Guid MenuItemId) : IDomainEvent;
