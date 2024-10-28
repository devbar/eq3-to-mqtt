namespace Devbar.EQ3.Lib.Converter;

public class TimeConverter : ITimeConverter
{
    public byte[] FromDateTime(DateTime datetime)
    {
        // 11 02 08 15 1f 05
        // + Byte 2 to 7: yy-mm-day hh-MM-ss in hex
        
        return [
            (byte)(datetime.Year % 100),
            (byte)datetime.Month,
            (byte)datetime.Day,
            (byte)datetime.Hour,
            (byte)datetime.Minute,
            (byte)datetime.Second
        ];
    }
}