using Ramendo.Domain.Common;

namespace Ramendo.Domain.Aggregates.Shops.Events;

public sealed record ShopVerifiedEvent(Guid ShopId) : IDomainEvent;
