using Ramendo.Domain.Common;

namespace Ramendo.Domain.Aggregates.InvitationCodes;

public sealed class InvitationCode : AggregateRoot
{
    public string Code { get; private set; } = null!;
    public Guid CreatedById { get; private set; }
    public bool IsActive { get; private set; }
    public int MaxUses { get; private set; }
    public int UsedCount { get; private set; }
    public DateTime? ExpiresAt { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    private InvitationCode() { }

    public static InvitationCode Create(string code, Guid createdById, int maxUses, DateTime? expiresAt) => new()
    {
        Id = Guid.NewGuid(), Code = code, CreatedById = createdById,
        IsActive = true, MaxUses = maxUses, UsedCount = 0,
        ExpiresAt = expiresAt, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow
    };

    public bool IsValid() =>
        IsActive && UsedCount < MaxUses && (ExpiresAt == null || ExpiresAt > DateTime.UtcNow);

    public void Use()
    {
        if (!IsValid()) throw new InvalidOperationException("Invitation code is not valid.");
        UsedCount++;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Toggle() { IsActive = !IsActive; UpdatedAt = DateTime.UtcNow; }
}
