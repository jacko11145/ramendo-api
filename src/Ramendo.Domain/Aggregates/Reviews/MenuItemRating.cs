using Ramendo.Domain.Common;

namespace Ramendo.Domain.Aggregates.Reviews;

public sealed class MenuItemRating : Entity
{
    public float Rating { get; private set; }
    public string? Comment { get; private set; }
    public Guid UserId { get; private set; }
    public Guid MenuItemId { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    private readonly List<ReviewOption> _reviewOptions = [];
    public IReadOnlyList<ReviewOption> ReviewOptions => _reviewOptions.AsReadOnly();

    private MenuItemRating() { }

    public static MenuItemRating Create(Guid userId, Guid menuItemId, float rating, string? comment) => new()
    {
        Id = Guid.NewGuid(), UserId = userId, MenuItemId = menuItemId,
        Rating = rating, Comment = comment,
        CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow
    };

    public void Update(float rating, string? comment, IEnumerable<ReviewOption> options)
    {
        Rating = rating; Comment = comment;
        _reviewOptions.Clear();
        _reviewOptions.AddRange(options);
        UpdatedAt = DateTime.UtcNow;
    }

    public void AddReviewOption(ReviewOption opt) => _reviewOptions.Add(opt);
}
