using MediatR;

namespace Ramendo.Application.Users.Commands;

public sealed record SetVipMembershipCommand(Guid UserId, bool IsVIP, DateTime? MembershipExpiry) : IRequest;
