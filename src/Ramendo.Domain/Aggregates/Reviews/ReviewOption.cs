using Ramendo.Domain.Common;

namespace Ramendo.Domain.Aggregates.Reviews;

public sealed class ReviewOption : Entity
{
    public Guid RatingId { get; private set; }
    public Guid OptionValueId { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    private ReviewOption() { }

    public static ReviewOption Create(Guid ratingId, Guid optionValueId) => new()
    {
        Id = Guid.NewGuid(), RatingId = ratingId, OptionValueId = optionValueId,
        CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow
    };
}
