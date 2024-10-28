using System.Data;
using System.Text;
using Devbar.EQ3.Worker.Manager;
using Devbar.EQ3.Worker.Publisher;
using Newtonsoft.Json;
using RabbitMQ.Client.Events;

namespace Devbar.EQ3.Worker.Receiver;

public class OpenWindowReceiver(
    IConfiguration configuration,
    IThermostatManager thermostatManager,
    ILogger<StatusReceiver> logger,
    IStatusPublisher statusPublisher) : AbstractReceiver(configuration, thermostatManager, logger), IReceiver
{
    private readonly IThermostatManager _thermostatManager = thermostatManager;
    protected override string Command => "{0}/{1}/openwindow/command";
    protected override string Tag => "openwindow";

    protected override async Task Receive(object sender, BasicDeliverEventArgs args)
    {
        try {
            var thermostat = _thermostatManager.GetThermostat(args.ConsumerTag);

            var body = Encoding.UTF8.GetString(args.Body.ToArray());

            var response = JsonConvert.DeserializeObject<dynamic>(body);

            if (response == null) { throw new NoNullAllowedException("The request for window open is null"); }

            if (response.Temperature == null) { throw new NotSupportedException("Thew request for window open needs a temperature"); }

            if (response.Minutes == null) { throw new NotSupportedException("The response for window open needs minutes"); }

            var status = await thermostat.SetWindowOpenModeAsync(
                temperature: (decimal)response.Temperature,
                minutes: (byte)response.Minutes
            );

            await statusPublisher.Publish(thermostat, status);
        } catch (Exception ex) { logger.LogError(ex, ex.Message); }
    }
}