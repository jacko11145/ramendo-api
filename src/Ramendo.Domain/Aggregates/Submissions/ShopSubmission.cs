using Ramendo.Domain.Common;

namespace Ramendo.Domain.Aggregates.Submissions;

public sealed class ShopSubmission : AggregateRoot
{
    public string Name { get; private set; } = null!;
    public string? Description { get; private set; }
    public string City { get; private set; } = null!;
    public string District { get; private set; } = null!;
    public string DetailAddress { get; private set; } = null!;
    public string? Phone { get; private set; }
    public string? Website { get; private set; }
    public string? FacebookPageId { get; private set; }
    public List<string> Images { get; private set; } = [];
    public List<string> Types { get; private set; } = [];
    public SubmissionStatus Status { get; private set; }
    public string? Feedback { get; private set; }
    public Guid UserId { get; private set; }
    public Guid? ApprovedById { get; private set; }
    public Guid? ApprovedShopId { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    private ShopSubmission() { }

    public static ShopSubmission Create(Guid userId, string name, string? description,
        string city, string district, string detailAddress, string? phone, string? website,
        string? facebookPageId, string[] images, string[] types) => new()
    {
        Id = Guid.NewGuid(), UserId = userId, Name = name, Description = description,
        City = city, District = district, DetailAddress = detailAddress,
        Phone = phone, Website = website, FacebookPageId = facebookPageId,
        Images = [.. images], Types = [.. types],
        Status = SubmissionStatus.Pending,
        CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow
    };

    public void Approve(Guid approvedById, Guid approvedShopId)
    {
        Status = SubmissionStatus.Approved;
        ApprovedById = approvedById;
        ApprovedShopId = approvedShopId;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Reject(Guid approvedById, string? feedback)
    {
        Status = SubmissionStatus.Rejected;
        ApprovedById = approvedById;
        Feedback = feedback;
        UpdatedAt = DateTime.UtcNow;
    }
}
