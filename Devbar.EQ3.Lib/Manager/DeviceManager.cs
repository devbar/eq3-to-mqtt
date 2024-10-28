using System.Data;
using Linux.Bluetooth;
using Linux.Bluetooth.Extensions;

namespace Devbar.EQ3.Lib.Manager;

public class DeviceManager(string controllerAddress) : IDeviceManager
{
    private IDictionary<string,Device>? _devices;
    
    public async Task<Device?> GetDevice(string address)
    {
        _devices ??= await GetDevices();
        
        return _devices.TryGetValue(address, out var device) ? device : null;
    }

    private async Task<IDictionary<string,Device>> GetDevices()
    {
        var adapters = await BlueZManager.GetAdaptersAsync();

        Adapter? adapter = null;

        foreach(var a in adapters){
            if(await a.GetAddressAsync() == controllerAddress)
            {
                adapter = a;
            }
        }

        if (adapter == null) {
            throw new NoNullAllowedException("The adapter was not found or is null");
        }

        var devices = await adapter.GetDevicesAsync();

        var result = new Dictionary<string, Device>();
        
        foreach (var d in devices) {
            result.Add(await d.GetAddressAsync(), d);
        }

        return result;
    }
}