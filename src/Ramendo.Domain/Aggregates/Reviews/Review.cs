using Ramendo.Domain.Aggregates.Reviews.Events;
using Ramendo.Domain.Aggregates.Users;
using Ramendo.Domain.Common;

namespace Ramendo.Domain.Aggregates.Reviews;

public sealed class Review : AggregateRoot
{
    public float Rating { get; private set; }
    public string? Content { get; private set; }
    public DateOnly? VisitDate { get; private set; }
    public List<string> Images { get; private set; } = [];
    public Guid UserId { get; private set; }
    public User? User { get; private set; }
    public Guid RamenShopId { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    private Review() { }

    public static Review Create(Guid userId, Guid shopId, float rating, string? content, IEnumerable<string> images, DateOnly? visitDate = null)
    {
        var review = new Review
        {
            Id = Guid.NewGuid(), UserId = userId, RamenShopId = shopId,
            Rating = rating, Content = content, Images = [.. images], VisitDate = visitDate,
            CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow
        };
        review.RaiseDomainEvent(new ReviewCreatedEvent(review.Id, shopId, userId, rating));
        return review;
    }

    public void Update(float rating, string? content, IEnumerable<string> images)
    {
        Rating = rating; Content = content; Images = [.. images]; UpdatedAt = DateTime.UtcNow;
    }
}
