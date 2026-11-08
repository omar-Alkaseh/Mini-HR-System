using System.Collections.Frozen;

namespace Domain.Common.Constant;

public static class WeekendConstants
{
    public static readonly FrozenSet<DayOfWeek> Days =
        new[]
        {
            DayOfWeek.Friday,
            DayOfWeek.Saturday
        }.ToFrozenSet();

    public static bool IsWeekend(DateOnly date) => 
        Days.Contains(date.DayOfWeek);
}