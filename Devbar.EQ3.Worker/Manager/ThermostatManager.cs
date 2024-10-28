using System.Collections.Concurrent;
using System.Text.RegularExpressions;
using Devbar.EQ3.Lib;
using Devbar.EQ3.Lib.Converter;
using Devbar.EQ3.Lib.Exception;
using Devbar.EQ3.Lib.Manager;
using Devbar.EQ3.Stub;

namespace Devbar.EQ3.Worker.Manager;

public partial class ThermostatManager : IThermostatManager
{
    private readonly IDeviceManager _deviceManager;
    private readonly IEventConverter _eventConverter;
    private readonly IDayOfWeekConverter _dayOfWeekConverter;
    private readonly IStatusConverter _statusConverter;
    private readonly ITimeConverter _timeConverter;
    private readonly ConcurrentDictionary<string, IThermostat> _thermostats = [];

    [GeneratedRegex("_.*$")]
    private static partial Regex ConsumerPostfix();

    public ThermostatManager
    (
        string[] devices,
        IDeviceManager deviceManager,
        IEventConverter eventConverter,
        IDayOfWeekConverter dayOfWeekConverter,
        IStatusConverter statusConverter,
        ITimeConverter timeConverter
    )
    {
        _deviceManager = deviceManager;
        _eventConverter = eventConverter;
        _dayOfWeekConverter = dayOfWeekConverter;
        _statusConverter = statusConverter;
        _timeConverter = timeConverter;

        foreach (var t in devices.Select(CreateThermostat)) {
            var therm = t.Result;
            _thermostats.AddOrUpdate(therm.GetNameAsync().Result, therm, (_, v) => v);
        }
    }

    public IThermostat GetThermostat(string consumerTag)
    {
        var thermostat = _thermostats.GetValueOrDefault(ConsumerPostfix().Replace(consumerTag, ""));

        if (thermostat == default) { throw new ThermostatNotFoundException(consumerTag); }

        return thermostat;
    }

    public IEnumerable<IThermostat> GetAll()
    {
        return _thermostats.Values;
    }

    private async Task<IThermostat> CreateThermostat(string device)
    {
        if (device.StartsWith("stub")) { return new ThermostatStub(device); }

        var channel = new Channel(device, _deviceManager);

        await channel.InitAsync();

        return new Thermostat(
            channel,
            _eventConverter,
            _dayOfWeekConverter,
            _statusConverter,
            _timeConverter);
    }
}