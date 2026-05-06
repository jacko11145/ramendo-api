using System.Text.Json.Serialization;
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

    [JsonConstructor]
    public BusinessHours(DayHours? monday, DayHours? tuesday, DayHours? wednesday, DayHours? thursday, DayHours? friday, DayHours? saturday, DayHours? sunday)
    {
        Monday = monday; Tuesday = tuesday; Wednesday = wednesday; Thursday = thursday;
        Friday = friday; Saturday = saturday; Sunday = sunday;
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Monday; yield return Tuesday; yield return Wednesday;
        yield return Thursday; yield return Friday; yield return Saturday; yield return Sunday;
    }
}
