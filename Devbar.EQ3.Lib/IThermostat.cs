using System.Runtime.InteropServices.JavaScript;
using Devbar.EQ3.Lib.Response;

namespace Devbar.EQ3.Lib;

public interface IThermostat {
    Task<string> GetNameAsync();
    Task<IStatus> GetStatusAsync();
    Task<IStatus> StartBoostAsync();
    Task<IStatus> StopBoostAsync();
    Task<IStatus> SetTimeAsync(DateTime time);
    Task<IStatus> SetTemperatureAsync(decimal temperature);
    Task<IStatus> SwitchToEcoTemperatureAsync();
    Task<IStatus> SwitchToComfortTemperatureAsync();
    Task<IStatus> SetEcoAndComfortTemperatureAsync(decimal eco, decimal comfort);
    Task<IStatus> SwitchToManualModeAsync();
    Task<IStatus> SwitchToAutoModeAsync();
    Task<IStatus> LockAsync();
    Task<IStatus> UnLockAsync();
    Task<IStatus> SetWindowOpenModeAsync(decimal temperature, byte minutes);
    Task<IStatus> SetVacationMode(decimal temp, DateTime until);
    Task<Response.Timer> GetTimer(DayOfWeek day);
    Task<DayOfWeek> SetTimer(Response.Timer timer);
}