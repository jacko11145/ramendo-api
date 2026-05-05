using Ramendo.Domain.Common;

namespace Ramendo.Domain.Aggregates.Users.Events;

public sealed record UserRegisteredEvent(Guid UserId, string Email) : IDomainEvent;
