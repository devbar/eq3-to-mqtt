using Devbar.EQ3.Lib;

namespace Devbar.EQ3.Worker.Manager;

public interface IThermostatManager
{
    IThermostat GetThermostat(string consumerTag);
    IEnumerable<IThermostat> GetAll();
}