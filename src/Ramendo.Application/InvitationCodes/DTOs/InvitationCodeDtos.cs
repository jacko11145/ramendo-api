namespace Ramendo.Application.InvitationCodes.DTOs;

public sealed record InvitationCodeDto(
    string Code, string CreatedById, bool IsActive,
    int MaxUses, int UsedCount, DateTime? ExpiresAt, DateTime CreatedAt);

public sealed record CreateInvitationCodeDto(int MaxUses, DateTime? ExpiresAt);
