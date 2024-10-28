using System.Data;
using System.Text;
using Devbar.EQ3.Worker.Manager;
using Devbar.EQ3.Worker.Publisher;
using Newtonsoft.Json;
using RabbitMQ.Client.Events;

namespace Devbar.EQ3.Worker.Receiver;

public class EcoComfortTemperatureReceiver(
    IConfiguration configuration,
    IThermostatManager thermostatManager,
    ILogger<StatusReceiver> logger,
    IStatusPublisher statusPublisher) : AbstractReceiver(configuration, thermostatManager, logger), IReceiver
{
    private readonly IThermostatManager _thermostatManager = thermostatManager;
    protected override string Command => "{0}/{1}/eco_comfort/temperature/command";
    protected override string Tag => "eco-comfort-temperature";
    protected override async Task Receive(object sender, BasicDeliverEventArgs args)
    {
        try {
            var thermostat = _thermostatManager.GetThermostat(args.ConsumerTag);

            var body = Encoding.UTF8.GetString(args.Body.ToArray());

            var response = JsonConvert.DeserializeObject<dynamic>(body);

            if (response == null) { throw new NoNullAllowedException("The request for eco and comfort is null"); }

            if (response.EcoTemperature == null) { throw new NotSupportedException("The request for eco and comfort temp mode needs a ecoTemperature"); }

            if (response.ComfortTemperature == null) { throw new NotSupportedException("The request for eco and comfort temp mode needs a comfortTemperature"); }

            var status = await thermostat.SetEcoAndComfortTemperatureAsync(
                eco: (decimal)response.EcoTemperature,
                comfort: (decimal)response.ComfortTemperature
            );

            await statusPublisher.Publish(thermostat, status);
        } catch (Exception ex) { logger.LogError(ex, ex.Message); }
    }
}