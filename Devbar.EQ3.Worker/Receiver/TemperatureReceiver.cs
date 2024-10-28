using System.Globalization;
using System.Text;
using Devbar.EQ3.Worker.Manager;
using Devbar.EQ3.Worker.Publisher;
using RabbitMQ.Client.Events;

namespace Devbar.EQ3.Worker.Receiver;

public class TemperatureReceiver(
    IConfiguration configuration,
    IThermostatManager thermostatManager,
    ILogger<StatusReceiver> logger,
    IStatusPublisher statusPublisher) : AbstractReceiver(configuration, thermostatManager, logger), IReceiver
{
    private readonly IThermostatManager _thermostatManager = thermostatManager;
    protected override string Command => "{0}/{1}/temperature/command";
    protected override string Tag => "temperature";

    protected override async Task Receive(object sender, BasicDeliverEventArgs args)
    {
        try {
            var thermostat = _thermostatManager.GetThermostat(args.ConsumerTag);

            var body = Encoding.UTF8.GetString(args.Body.ToArray());

            var temperature = decimal.Parse(body, CultureInfo.InvariantCulture);

            var status = await thermostat.SetTemperatureAsync(temperature);

            await statusPublisher.Publish(thermostat, status);
        } catch (Exception ex) { logger.LogError(ex, ex.Message); }
    }
}