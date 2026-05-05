using Ramendo.Domain.Aggregates.Users.Events;
using Ramendo.Domain.Common;

namespace Ramendo.Domain.Aggregates.Users;

public sealed class User : AggregateRoot
{
    public string? Name { get; private set; }
    public string Email { get; private set; } = null!;
    public string? PasswordHash { get; private set; }
    public string? Image { get; private set; }
    public UserRole Role { get; private set; }
    public bool IsActive { get; private set; }
    public string? InvitedByCode { get; private set; }
    public ExperiencePoints Experience { get; private set; } = ExperiencePoints.Zero;
    public VIPMembership VIP { get; private set; } = VIPMembership.None;
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    private User() { }

    public static User Create(string email, string? passwordHash, string? name, string? invitedByCode)
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = email,
            PasswordHash = passwordHash,
            Name = name,
            Role = UserRole.User,
            IsActive = true,
            InvitedByCode = invitedByCode,
            Experience = ExperiencePoints.Zero,
            VIP = VIPMembership.None,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
        };
        user.RaiseDomainEvent(new UserRegisteredEvent(user.Id, email));
        return user;
    }

    public void AwardExperience(int points)
    {
        Experience = Experience.Add(points);
        UpdatedAt = DateTime.UtcNow;
        RaiseDomainEvent(new ExperienceAwardedEvent(Id, points, Experience.Points));
    }

    public void AdjustExperience(int points)
    {
        Experience = ExperiencePoints.Of(points);
        UpdatedAt = DateTime.UtcNow;
    }

    public void ChangeRole(UserRole role)
    {
        Role = role;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetVIP(bool isVip, DateTime? expiry)
    {
        VIP = isVip ? VIPMembership.Active(expiry) : VIPMembership.None;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetActive(bool active)
    {
        IsActive = active;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetImage(string? imageUrl)
    {
        Image = imageUrl;
        UpdatedAt = DateTime.UtcNow;
    }
}
