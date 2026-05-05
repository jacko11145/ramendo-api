using Ramendo.Domain.Common;

namespace Ramendo.Domain.Aggregates.Users;

public sealed class VIPMembership : ValueObject
{
    public bool IsVIP { get; }
    public DateTime? MembershipExpiry { get; }

    public bool IsActive => IsVIP && (MembershipExpiry == null || MembershipExpiry > DateTime.UtcNow);

    private VIPMembership(bool isVIP, DateTime? membershipExpiry)
    {
        IsVIP = isVIP;
        MembershipExpiry = membershipExpiry;
    }

    public static VIPMembership None => new(false, null);
    public static VIPMembership Active(DateTime? membershipExpiry) => new(true, membershipExpiry);

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return IsVIP;
        yield return MembershipExpiry;
    }
}
