using Linux.Bluetooth;

namespace Devbar.EQ3.Lib.Manager;

public interface IDeviceManager
{
    Task<Device?> GetDevice(string address);
}