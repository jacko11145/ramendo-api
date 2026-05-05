using Ramendo.Domain.Common;

namespace Ramendo.Domain.Aggregates.Shops;

public sealed record DayHours(bool IsOpen, bool IsSplit, string? Open, string? Close, string? LunchOpen, string? LunchClose, string? DinnerOpen, string? DinnerClose);

public sealed class BusinessHours : ValueObject
{
    public DayHours? Monday { get; }
    public DayHours? Tuesday { get; }
    public DayHours? Wednesday { get; }
    public DayHours? Thursday { get; }
    public DayHours? Friday { get; }
    public DayHours? Saturday { get; }
    public DayHours? Sunday { get; }

    public BusinessHours(DayHours? mon, DayHours? tue, DayHours? wed, DayHours? thu, DayHours? fri, DayHours? sat, DayHours? sun)
    {
        Monday = mon; Tuesday = tue; Wednesday = wed; Thursday = thu;
        Friday = fri; Saturday = sat; Sunday = sun;
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Monday; yield return Tuesday; yield return Wednesday;
        yield return Thursday; yield return Friday; yield return Saturday; yield return Sunday;
    }
}
