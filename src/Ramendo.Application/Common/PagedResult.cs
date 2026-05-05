namespace Ramendo.Application.Common;

public sealed class PagedResult<T>
{
    public IReadOnlyList<T> Items { get; init; } = [];
    public int Total { get; init; }
    public int Page { get; init; }
    public int Limit { get; init; }
    public int TotalPages => (int)Math.Ceiling((double)Total / Limit);
    public bool HasNext => Page < TotalPages;
    public bool HasPrev => Page > 1;

    public static PagedResult<T> Create(IReadOnlyList<T> items, int total, int page, int limit) =>
        new() { Items = items, Total = total, Page = page, Limit = limit };
}
