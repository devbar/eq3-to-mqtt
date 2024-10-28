using Devbar.EQ3.Lib;
using Devbar.EQ3.Lib.Response;
using Timer = Devbar.EQ3.Lib.Response.Timer;

namespace Devbar.EQ3.Stub;

public class ThermostatStub(string name) : IThermostat
{
    public async Task<string> GetNameAsync()
    {
        return await Task.FromResult("eq3-" + name);
    }

    public async Task<IStatus> GetStatusAsync()
    {
        return await Task.FromResult(new Status(Lib.Enum.Mode.Auto, 0, 0, null, 0, 0, 0, 0, 100));
    }

    public async Task<IStatus> StartBoostAsync()
    {
        return await Task.FromResult(new Status(Lib.Enum.Mode.Boost, 0, 0, null, 0, 0, 0, 0, 100));
    }

    public async Task<IStatus> StopBoostAsync()
    {
        return await Task.FromResult(new Status(Lib.Enum.Mode.Auto, 0, 0, null, 0, 0, 0, 0, 100));
    }

    public async Task<IStatus> SetTimeAsync(DateTime time)
    {
        return await Task.FromResult(new Status(Lib.Enum.Mode.Auto, 0, 0, null, 0, 0, 0, 0, 100));
    }

    public async Task<IStatus> SetTemperatureAsync(decimal temperature)
    {
        return await Task.FromResult(new Status(Lib.Enum.Mode.Auto, 0, temperature, null, 0, 0, 0, 0, 100));
    }

    public async Task<IStatus> SwitchToEcoTemperatureAsync()
    {
        return await Task.FromResult(new Status(Lib.Enum.Mode.Auto, 0, 0, null, 0, 0, 0, 0, 100));
    }

    public async Task<IStatus> SwitchToComfortTemperatureAsync()
    {
        return await Task.FromResult(new Status(Lib.Enum.Mode.Auto, 0, 0, null, 0, 0, 0, 0, 100));
    }

    public async Task<IStatus> SetEcoAndComfortTemperatureAsync(decimal eco, decimal comfort)
    {
        return await Task.FromResult(new Status(Lib.Enum.Mode.Auto, 0, 0, null, 0, 0, comfort, eco, 100));
    }

    public async Task<IStatus> SwitchToManualModeAsync()
    {
        return await Task.FromResult(new Status(Lib.Enum.Mode.Manual, 0, 0, null, 0, 0, 0, 0, 100));
    }

    public async Task<IStatus> SwitchToAutoModeAsync()
    {
        return await Task.FromResult(new Status(Lib.Enum.Mode.Auto, 0, 0, null, 0, 0, 0, 0, 100));
    }

    public async Task<IStatus> LockAsync()
    {
        return await Task.FromResult(new Status(Lib.Enum.Mode.Locked, 0, 0, null, 0, 0, 0, 0, 100));
    }

    public async Task<IStatus> UnLockAsync()
    {
        return await Task.FromResult(new Status(Lib.Enum.Mode.Manual, 0, 0, null, 0, 0, 0, 0, 100));
    }

    public async Task<IStatus> SetWindowOpenModeAsync(decimal temperature, byte minutes)
    {
        return await Task.FromResult(new Status(Lib.Enum.Mode.OpenWindow, 0, 0, null, 0, 0, 0, 0, 100));
    }

    public async Task<IStatus> SetVacationMode(decimal temp, DateTime until)
    {
        return await Task.FromResult(new Status(Lib.Enum.Mode.Vacation, 0, temp, until, 0, 0, 0, 0, 100));
    }

    public async Task<Timer> GetTimer(DayOfWeek day)
    {
        return await Task.FromResult(new Timer(
            day,
            new Event(12.5m, 6, 30)));
    }

    public async Task<DayOfWeek> SetTimer(Timer timer)
    {
        return await Task.FromResult(timer.Day);
    }
}