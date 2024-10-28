using Devbar.EQ3.Lib;
using Devbar.EQ3.Lib.Response;

namespace Devbar.EQ3.Worker.Publisher;

public interface IStatusPublisher
{
    Task Publish(IThermostat thermostat, IStatus status);
}