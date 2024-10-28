namespace Devbar.EQ3.Lib.Response;

public class Timer(
    DayOfWeek day,
    Event? event1 = null,
    Event? event2 = null,
    Event? event3 = null,
    Event? event4 = null,
    Event? event5 = null,
    Event? event6 = null,
    Event? event7 = null)
    : ITimer
{
    public DayOfWeek Day { get; } = day;

    public Event Event1 { get; } = event1 ?? Event.Default;
    public Event Event2 { get; } = event2 ?? Event.Default;
    public Event Event3 { get; } = event3 ?? Event.Default;
    public Event Event4 { get; } = event4 ?? Event.Default;
    public Event Event5 { get; } = event5 ?? Event.Default;
    public Event Event6 { get; } = event6 ?? Event.Default;
    public Event Event7 { get; } = event7 ?? Event.Default;
}