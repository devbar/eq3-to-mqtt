using Devbar.EQ3.Lib.Exception;

namespace Devbar.EQ3.Worker.Parser;

public class DateTimeParser : IDateTimeParser
{
    public DateTime Parse(string val)
    {
        if (DateTime.TryParse(val, null, out var dateTime)) { return dateTime; }

        throw new InvalidDatetimeException(val);
    }
}