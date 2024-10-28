using Devbar.EQ3.Lib;
using Devbar.EQ3.Lib.Response;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace Devbar.EQ3.Worker.Publisher;

public class StatusPublisher(
    IConfiguration configuration,
    ILogger<Worker> logger,
    IConnection connection) 
    : AbstractPublisher(
        configuration,
        logger,
        connection), IStatusPublisher
{
    private const string StateTopic = "{0}/{1}/status";

    public async Task Publish(IThermostat thermostat, IStatus status)
    {
        try {
            var data = JsonConvert.SerializeObject(status);

            await Publish(thermostat, StateTopic, data);
        } catch (JsonException exp) {
            logger.LogError(exp, "Error serializing status object");
            throw;
        }
    }
}