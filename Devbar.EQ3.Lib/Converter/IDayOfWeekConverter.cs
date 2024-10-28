namespace Devbar.EQ3.Lib.Converter;

public interface IDayOfWeekConverter
{
    byte FromDayOfWeek(DayOfWeek day);
    DayOfWeek FromByte(byte day);
}