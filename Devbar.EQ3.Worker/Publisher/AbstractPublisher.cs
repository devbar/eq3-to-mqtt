using System.Text;
using Devbar.EQ3.Lib;
using RabbitMQ.Client;

namespace Devbar.EQ3.Worker.Publisher;

public abstract class AbstractPublisher(
    IConfiguration configuration,
    ILogger logger,
    IConnection connection)
{
    private readonly string _exchange = configuration.GetValue<string>("Mqtt:Exchange") ?? throw new Exception("Add a Mqtt:Exchange for the configuration");
    private readonly string _routingKey = configuration.GetValue<string>("Mqtt:RoutingKey") ?? throw new Exception("Add a Mqtt:RoutingKey for the configuration");

    protected async Task Publish(IThermostat thermostat, string topic, string data, int retry = 3)
    {
        using var model = connection.CreateModel();

        try {
            var props = model.CreateBasicProperties();
            props.ContentType = "application/json";
            model.BasicPublish(_exchange, string.Format(topic, _routingKey, await thermostat.GetNameAsync()).Replace("/", "."), props, Encoding.UTF8.GetBytes(data));
        } catch (Exception exp) {
            logger.LogError(exp, "Error publishing data (retry={0})", retry);

            if (retry == 0) { throw; }

            await Publish(thermostat, data, topic, retry - 1);
        } finally { model?.Close(); }
    }
}