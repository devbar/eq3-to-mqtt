using Devbar.EQ3.Lib;

namespace Devbar.EQ3.Worker.Publisher;

public interface ITimerPublisher
{
    Task Publish(IThermostat thermostat, Devbar.EQ3.Lib.Response.ITimer timer);
}