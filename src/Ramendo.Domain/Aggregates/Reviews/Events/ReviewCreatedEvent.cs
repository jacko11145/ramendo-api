using Ramendo.Domain.Common;

namespace Ramendo.Domain.Aggregates.Reviews.Events;

public sealed record ReviewCreatedEvent(Guid ReviewId, Guid ShopId, Guid UserId, float Rating) : IDomainEvent;
