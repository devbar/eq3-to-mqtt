namespace Devbar.EQ3.Lib.Response;

public class Event
{
    public Event(decimal temperature, ushort hour, ushort minute)
    {
        Temperature = temperature;
        this.Hour = hour;
        this.Minute = minute;
    }

    public static Event Default = new Event(0m, 0, 0);

    public decimal Temperature { get; }
    public ushort Hour { get; }
    public ushort Minute { get; }
}