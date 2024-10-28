using System.Data;
using Devbar.EQ3.Lib.Manager;
using Linux.Bluetooth;
using Linux.Bluetooth.Extensions;
using Tmds.DBus;

public class Channel : IChannel, IDisposable
{
    private readonly string _address;
    private readonly IDeviceManager _deviceManager;

    private const int DefaultResponseStatus = 1500;

    private readonly Dictionary<int, Tuple<string, string>> _addresses = new() {
        { 0x0411, new Tuple<string, string>("3e135142-654f-9090-134a-a6ff5bb77046", "3fa4585a-ce4a-3bad-db4b-b8df8179ea09") },
        { 0x0421, new Tuple<string, string>("3e135142-654f-9090-134a-a6ff5bb77046", "d0e8434d-cd29-0996-af41-6c90f4e0eb2a") },
        { 0x0311, new Tuple<string, string>("0000180a-0000-1000-8000-00805f9b34fb", "00002a29-0000-1000-8000-00805f9b34fb") }
    };

    private readonly TimeSpan _defaultTimeout = TimeSpan.FromSeconds(15);

    private readonly object _deviceLoc = new();

    private Device? _deviceInt;

    private bool _connected;

    private Device device {
        get {
            lock (_deviceLoc) {
                if (_deviceInt == null) { return _deviceInt = _deviceManager.GetDevice(_address).Result ?? throw new NoNullAllowedException("Device was not found"); }

                return _deviceInt;
            }
        }
    }

    private Task OnConnected(Device sender, BlueZEventArgs eventArgs)
    {
        _connected = true;
        return Task.CompletedTask;
    }

    private Task OnDisconnected(Device sender, BlueZEventArgs eventArgs)
    {
        _connected = false;
        return Task.CompletedTask;
    }

    public event EventHandler<byte[]>? ValueChanged;

    public Channel(string address, IDeviceManager deviceManager)
    {
        if (string.IsNullOrEmpty(address)) { throw new ArgumentNullException(nameof(address)); }

        _address = address;
        _deviceManager = deviceManager;

        device.Connected += OnConnected;
        device.Disconnected += OnDisconnected;
    }

    private static void Log(string message)
    {
        Console.WriteLine(message);
    }
    
    private async Task<GattCharacteristic> GetCharacteristicByHandle(int handle)
    {
        var service = await device.GetServiceAsync(_addresses[handle].Item1);
        var characteristic = await service.GetCharacteristicAsync(_addresses[handle].Item2);

        return characteristic;
    }

    private async Task ConnectAsync()
    {
        await device.ConnectAsync();
    }

    private static async Task WaitNotificationResponse(Task task)
    {
        try { await task; } catch (TaskCanceledException) { }
    }

    private async Task<byte[]> GetCharacterValueAsync(int charHandle)
    {
        try {
            if (!_connected) await ConnectAsync();
        } catch (DBusException exp) {
            Log($"Connection failed with error: {exp.Message}");
            return [];
        }

        using var characteristic = await GetCharacteristicByHandle(charHandle);

        var result = await characteristic.ReadValueAsync(_defaultTimeout);

        return result;
    }

    private static byte[] ConvertIntToByte(short v)
    {
        var b = BitConverter.GetBytes(v);

        return BitConverter.IsLittleEndian ? b.Reverse().ToArray() : b;
    }

    private async Task<byte[]> GetNotifyValueAsync(int charHandle, int notificationHandle, short command)
        => await GetNotifyValueAsync(charHandle, notificationHandle, ConvertIntToByte(command));

    private async Task<byte[]> GetNotifyValueAsync(int charHandle, int notificationHandle, byte command)
        => await GetNotifyValueAsync(charHandle, notificationHandle, new byte[] { command });

    private async Task<byte[]> GetNotifyValueAsync(int charHandle, int notificationHandle, byte[] command)
    {
        try {
            if (!_connected) await ConnectAsync();
        } catch (DBusException exp) {
            Log($"Connection failed with error: {exp.Message}");
            return [];
        }

        using var characteristic = await GetCharacteristicByHandle(charHandle);
        using var notification = await GetCharacteristicByHandle(notificationHandle);

        byte[] result = [];

        var source = new CancellationTokenSource();

        var task = Task.Delay(DefaultResponseStatus, source.Token);

        notification.Value += ValueChanged;

        await characteristic.WriteValueAsync(command, new Dictionary<string, object>());

        await WaitNotificationResponse(task);

        notification.Value -= ValueChanged;

        foreach (var r in result) { Console.Write(r + " "); }

        Console.WriteLine();

        return result;

        Task ValueChanged(GattCharacteristic s, GattCharacteristicValueEventArgs e)
        {
            result = e.Value;
            source.Cancel();
            return Task.CompletedTask;
        }
    }

    public void Dispose()
    {
        if (_connected)
            device.DisconnectAsync().RunSynchronously();
    }

    public async Task<byte[]> GetStatusAsync()
        => await GetNotifyValueAsync(0x0411, 0x0421, 3);

    public async Task<byte[]> GetNameAsync()
        => await GetCharacterValueAsync(0x0311);

    public async Task<byte[]> StartBoostAsync()
        => await GetNotifyValueAsync(0x0411, 0x0421, 0x45ff);

    public async Task<byte[]> StopBoostAsync()
        => await GetNotifyValueAsync(0x0411, 0x0421, 0x4500);

    public async Task<byte[]> SetTimeAsync(byte[] data)
        => await GetNotifyValueAsync(0x0411, 0x0421, [.. (new byte[] { 0x03 }), .. data]);

    public async Task<byte[]> SetTemperatureAsync(byte data)
        => await GetNotifyValueAsync(0x0411, 0x0421, [0x41, data]);

    public async Task<byte[]> SwitchToEcoTemperature()
        => await GetNotifyValueAsync(0x0411, 0x0421, [0x44]);

    public async Task<byte[]> SwitchToComfortTemperature()
        => await GetNotifyValueAsync(0x0411, 0x0421, [0x43]);

    public async Task<byte[]> GetTimer(byte day)
        => await GetNotifyValueAsync(0x0411, 0x0421, [0x20, day]);

    public async Task<byte[]> SetTimer(byte day, byte[] data)
        => await GetNotifyValueAsync(0x0411, 0x0421, [.. (new byte[] { 0x10, day }), .. data]);

    public async Task<byte[]> SetEcoAndComfortTemperature(byte eco, byte comf)
        => await GetNotifyValueAsync(0x0411, 0x0421, [0x11, eco, comf]);

    public async Task<byte[]> SetWindowOpenMode(byte temp, byte minutes)
        => await GetNotifyValueAsync(0x0411, 0x0421, [0x14, temp, minutes]);

    public async Task<byte[]> SetOffsetTemperature(byte data)
        => await GetNotifyValueAsync(0x0411, 0x0421, [0x13, data]);

    public async Task<byte[]> SwitchToManualMode()
        => await GetNotifyValueAsync(0x0411, 0x0421, [0x40, 0x40]);

    public async Task<byte[]> SwitchToAutoMode()
        => await GetNotifyValueAsync(0x0411, 0x0421, [0x40, 0x00]);

    public async Task<byte[]> SetVacationMode(byte temp, byte day, byte year, byte hourMin, byte month)
        => await GetNotifyValueAsync(0x0411, 0x0421, [0x40, temp, day, year, hourMin, month]);

    public async Task<byte[]> Lock()
        => await GetNotifyValueAsync(0x0411, 0x0421, [0x80, 0x01]);

    public async Task<byte[]> UnLock()
        => await GetNotifyValueAsync(0x0411, 0x0421, [0x80, 0x01]);

    public async Task InitAsync()
    {
        var n = await GetCharacteristicByHandle(0x0421);

        n.Value += (_, e) => {
            ValueChanged?.Invoke(this, e.Value);
            return Task.CompletedTask;
        };
    }

    public string GetAddress()
    {
        return _address;
    }
}