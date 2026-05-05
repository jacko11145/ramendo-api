using Ramendo.Domain.Common;

namespace Ramendo.Domain.Aggregates.Users;

public sealed class ExperiencePoints : ValueObject
{
    public int Points { get; }
    public int Level => (int)Math.Floor(Points / 100.0) + 1;

    private ExperiencePoints(int points) => Points = points < 0 ? 0 : points;

    public static ExperiencePoints Of(int points) => new(points);
    public static ExperiencePoints Zero => new(0);

    public ExperiencePoints Add(int amount) => new(Points + amount);
    public ExperiencePoints Subtract(int amount) => new(Points - amount);

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Points;
    }
}
