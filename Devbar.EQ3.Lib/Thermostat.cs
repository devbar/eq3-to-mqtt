using System.Text;
using Devbar.EQ3.Lib.Converter;
using Devbar.EQ3.Lib.Response;
using Timer = Devbar.EQ3.Lib.Response.Timer;

namespace Devbar.EQ3.Lib;

public class Thermostat(
    IChannel channel, 
    IEventConverter eventConverter, 
    IDayOfWeekConverter dayOfWeekConverter, 
    IStatusConverter statusConverter,
    ITimeConverter timeConverter) : IThermostat
{
    private string? _name;

    public async Task<string> GetNameAsync()
    {
        if (_name != null) { return _name; }

        var rawData = await channel.GetNameAsync();

        return _name = $"{Encoding.UTF8.GetString(rawData)}-{channel.GetAddress().Replace(":", "")}";
    }

    public async Task<IStatus> GetStatusAsync()
    {
        var rawData = await channel.GetStatusAsync();

        return statusConverter.FromBytes(rawData);
    }

    public async Task<IStatus> StartBoostAsync()
    {
        var rawData = await channel.StartBoostAsync();

        return statusConverter.FromBytes(rawData);
    }

    public async Task<IStatus> StopBoostAsync()
    {
        var rawData = await channel.StopBoostAsync();

        return statusConverter.FromBytes(rawData);
    }

    public async Task<IStatus> SetTimeAsync(DateTime time)
    {
        var status = await channel.SetTimeAsync(timeConverter.FromDateTime(time));

        return statusConverter.FromBytes(status);
    }

    public async Task<IStatus> SetTemperatureAsync(decimal temperature)
    {
        var status = await channel.SetTemperatureAsync((byte)(temperature * 2m));

        return statusConverter.FromBytes(status);
    }

    public async Task<IStatus> SwitchToEcoTemperatureAsync()
    {
        var status = await channel.SwitchToEcoTemperature();

        return statusConverter.FromBytes(status);
    }

    public async Task<IStatus> SwitchToComfortTemperatureAsync()
    {
        var status = await channel.SwitchToComfortTemperature();

        return statusConverter.FromBytes(status);
    }

    public async Task<IStatus> SetEcoAndComfortTemperatureAsync(decimal eco, decimal comfort)
    {
        var status = await channel.SetEcoAndComfortTemperature((byte)(eco * 2m), (byte)(comfort * 2m));

        return statusConverter.FromBytes(status);
    }

    public async Task<IStatus> SwitchToManualModeAsync()
    {
        var status = await channel.SwitchToManualMode();

        return statusConverter.FromBytes(status);
    }

    public async Task<IStatus> SwitchToAutoModeAsync()
    {
        var status = await channel.SwitchToAutoMode();

        return statusConverter.FromBytes(status);
    }

    public async Task<IStatus> LockAsync()
    {
        var status = await channel.Lock();

        return statusConverter.FromBytes(status);
    }

    public async Task<IStatus> UnLockAsync()
    {
        var status = await channel.UnLock();

        return statusConverter.FromBytes(status);
    }

    public async Task<IStatus> SetWindowOpenModeAsync(decimal temperature, byte minutes)
    {
        var status = await channel.SetWindowOpenMode((byte)(temperature * 2m), minutes);

        return statusConverter.FromBytes(status);
    }

    public async Task<IStatus> SetVacationMode(decimal temp, DateTime until)
    {
        var status = await channel.SetVacationMode(
            (byte)(temp * 2m), 
            (byte)until.Day, 
            (byte)(until.Year - 2000), 
            (byte)until.Hour, 
            (byte)until.Month
        );

        return statusConverter.FromBytes(status);
    }

    public async Task<Timer> GetTimer(DayOfWeek day)
    {
        var timer = await channel.GetTimer(dayOfWeekConverter.FromDayOfWeek(day));

        var events = new List<Event> {
            eventConverter.FromBytes(timer[2], timer[3]),
            eventConverter.FromBytes(timer[4], timer[5]),
            eventConverter.FromBytes(timer[6], timer[7]),
            eventConverter.FromBytes(timer[8], timer[9]),
            eventConverter.FromBytes(timer[10], timer[11]),
            eventConverter.FromBytes(timer[12], timer[13]),
            eventConverter.FromBytes(timer[14], timer[15])
        };

        return new Timer(
            dayOfWeekConverter.FromByte(timer[1]),
            events[0],
            events[1],
            events[2],
            events[3],
            events[4],
            events[5],
            events[6]
        );
    }

    public async Task<DayOfWeek> SetTimer(Timer timer)
    {
        var t = new List<byte>();
        
        t.AddRange(eventConverter.FromEvent(timer.Event1));
        t.AddRange(eventConverter.FromEvent(timer.Event2));
        t.AddRange(eventConverter.FromEvent(timer.Event3));
        t.AddRange(eventConverter.FromEvent(timer.Event4));
        t.AddRange(eventConverter.FromEvent(timer.Event5));
        t.AddRange(eventConverter.FromEvent(timer.Event6));
        t.AddRange(eventConverter.FromEvent(timer.Event7));
            
        var result = await channel.SetTimer(dayOfWeekConverter.FromDayOfWeek(timer.Day), t.ToArray());

        return dayOfWeekConverter.FromByte(result[1]);
    }
}