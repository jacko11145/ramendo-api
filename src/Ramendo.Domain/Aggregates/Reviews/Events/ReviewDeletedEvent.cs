using Ramendo.Domain.Common;

namespace Ramendo.Domain.Aggregates.Reviews.Events;

public sealed record ReviewDeletedEvent(Guid ReviewId, Guid ShopId) : IDomainEvent;
