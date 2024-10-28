public interface IChannel
{
    string GetAddress();
    Task<byte[]> GetStatusAsync();
    Task<byte[]> GetNameAsync();
    Task<byte[]> StartBoostAsync();
    Task<byte[]> StopBoostAsync();
    Task<byte[]> SetTimeAsync(byte[] data);
    Task<byte[]> SetTemperatureAsync(byte data);
    Task<byte[]> SwitchToEcoTemperature();
    Task<byte[]> SwitchToComfortTemperature();
    Task<byte[]> GetTimer(byte day);
    Task<byte[]> SetTimer(byte day, byte[] data);
    Task<byte[]> SetEcoAndComfortTemperature(byte eco, byte comf);
    Task<byte[]> SetWindowOpenMode(byte temp, byte minutes);
    Task<byte[]> SwitchToManualMode();
    Task<byte[]> SwitchToAutoMode();
    Task<byte[]> SetVacationMode(byte temp, byte day, byte year, byte hourMin, byte month);
    Task<byte[]> Lock();
    Task<byte[]> UnLock();
    Task InitAsync();
}