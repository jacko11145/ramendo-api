namespace Ramendo.Domain.Aggregates.Shops;

public sealed record NewsItem(string Title, string Content, DateTime? StartDate, DateTime? EndDate, DateTime CreatedAt)
{
    public bool IsVisible(DateTime now) =>
        (StartDate == null || StartDate <= now) && (EndDate == null || EndDate >= now);
}
