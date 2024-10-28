namespace Devbar.EQ3.Lib.Converter;

public class DayOfWeekConverter : IDayOfWeekConverter
{
    public byte FromDayOfWeek(DayOfWeek day)
    {
        return day switch {
            DayOfWeek.Saturday => 0,
            DayOfWeek.Sunday => 1,
            DayOfWeek.Monday => 2,
            DayOfWeek.Tuesday => 3,
            DayOfWeek.Wednesday => 4,
            DayOfWeek.Thursday => 5,
            DayOfWeek.Friday => 6,
            _ => throw new ArgumentOutOfRangeException(nameof(day), day, "Day enum does not match a byte")
        };
    }

    public DayOfWeek FromByte(byte day)
    {
        return day switch {
            0 => DayOfWeek.Saturday,
            1 => DayOfWeek.Sunday,
            2 => DayOfWeek.Monday,
            3 => DayOfWeek.Tuesday,
            4 => DayOfWeek.Wednesday,
            5 => DayOfWeek.Thursday,
            6 => DayOfWeek.Friday,
            _ => throw new ArgumentOutOfRangeException(nameof(day), day, "Day enum does not match a byte")
        };
    }
}