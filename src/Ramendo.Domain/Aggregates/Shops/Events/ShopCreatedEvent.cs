using Ramendo.Domain.Common;

namespace Ramendo.Domain.Aggregates.Shops.Events;

public sealed record ShopCreatedEvent(Guid ShopId, string Name) : IDomainEvent;
