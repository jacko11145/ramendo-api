using Ramendo.Domain.Common;

namespace Ramendo.Domain.Aggregates.Users.Events;

public sealed record ExperienceAwardedEvent(Guid UserId, int PointsAwarded, int NewTotal) : IDomainEvent;
