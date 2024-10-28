using Devbar.EQ3.Lib;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace Devbar.EQ3.Worker.Publisher;

public class TimerPublisher(
    IConfiguration configuration,
    ILogger<Worker> logger,
    IConnection connection) 
    : AbstractPublisher(
        configuration, 
        logger, 
        connection), ITimerPublisher
{
    private const string TimerTopic = "{0}/{1}/timer";

    public async Task Publish(IThermostat thermostat, Devbar.EQ3.Lib.Response.ITimer timer)
    {
        try {
            var data = JsonConvert.SerializeObject(timer);

            await Publish(thermostat, TimerTopic, data);
        } catch (JsonException exp) {
            logger.LogError(exp, "Error serializing status object");
            throw;
        }
    }
}