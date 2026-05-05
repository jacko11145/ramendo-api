namespace Ramendo.Application.Users.DTOs;

public sealed record UserListDto(
    string Id, string? Name, string Email, string Role, bool IsActive,
    bool IsVIP, DateTime? MembershipExpiry, int ExperiencePoints, int Level,
    int ReviewCount, DateTime CreatedAt);

public sealed record UserDetailDto(
    string Id, string? Name, string Email, string? Image, string Role,
    bool IsActive, bool IsVIP, DateTime? MembershipExpiry,
    int ExperiencePoints, int Level, string? InvitedByCode, DateTime CreatedAt);

public sealed record UserLevelDto(int Level, int ExperiencePoints, int PointsToNextLevel, float Progress);

public sealed record UpdateUserRoleDto(string Role);

public sealed record SetVipDto(bool IsVIP, DateTime? MembershipExpiry);

public sealed record AdjustExperienceDto(int Points);
