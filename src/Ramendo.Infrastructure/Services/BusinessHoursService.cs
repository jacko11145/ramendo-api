using Ramendo.Domain.Aggregates.Shops;

namespace Ramendo.Infrastructure.Services;

public static class BusinessHoursService
{
    private static readonly TimeZoneInfo TaiwanTz = TimeZoneInfo.FindSystemTimeZoneById("Asia/Taipei");

    public static bool IsOpenNow(BusinessHours? hours)
    {
        if (hours == null) return false;
        var now = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TaiwanTz);
        var day = GetTodayHours(hours, now.DayOfWeek);
        return day != null && IsTimeInRange(day, now.TimeOfDay);
    }

    private static DayHours? GetTodayHours(BusinessHours h, DayOfWeek dow) => dow switch
    {
        DayOfWeek.Monday => h.Monday,
        DayOfWeek.Tuesday => h.Tuesday,
        DayOfWeek.Wednesday => h.Wednesday,
        DayOfWeek.Thursday => h.Thursday,
        DayOfWeek.Friday => h.Friday,
        DayOfWeek.Saturday => h.Saturday,
        DayOfWeek.Sunday => h.Sunday,
        _ => null
    };

    private static bool IsTimeInRange(DayHours day, TimeSpan now)
    {
        if (!day.IsOpen) return false;
        if (day.IsSplit)
        {
            return (InRange(day.LunchOpen, day.LunchClose, now) ||
                    InRange(day.DinnerOpen, day.DinnerClose, now));
        }
        return InRange(day.Open, day.Close, now);
    }

    private static bool InRange(string? open, string? close, TimeSpan now)
    {
        if (!TimeSpan.TryParse(open, out var o) || !TimeSpan.TryParse(close, out var c)) return false;
        return now >= o && now <= c;
    }
}
