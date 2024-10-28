using System.Data;
using System.Text;
using Devbar.EQ3.Worker.Manager;
using Devbar.EQ3.Worker.Publisher;
using Newtonsoft.Json;
using RabbitMQ.Client.Events;

namespace Devbar.EQ3.Worker.Receiver;

public class VacationReceiver(
    IConfiguration configuration,
    IThermostatManager thermostatManager,
    ILogger<StatusReceiver> logger,
    IStatusPublisher statusPublisher) : AbstractReceiver(configuration, thermostatManager, logger), IReceiver
{
    private readonly IThermostatManager _thermostatManager = thermostatManager;
    protected override string Command => "{0}/{1}/vacation/command";
    protected override string Tag => "vacation";
    protected override async Task Receive(object sender, BasicDeliverEventArgs args)
    {
        try {
            var thermostat = _thermostatManager.GetThermostat(args.ConsumerTag);

            var body = Encoding.UTF8.GetString(args.Body.ToArray());

            var response = JsonConvert.DeserializeObject<dynamic>(body);

            if (response == null) { throw new NoNullAllowedException("The request for window open is null"); }

            if (response.Temperature == null) { throw new NotSupportedException("The request for vacation mode needs a temperature"); }

            if (response.Until == null) { throw new NotSupportedException("The request for vacation mode needs a until"); }

            var status = await thermostat.SetVacationMode(
                temp: (decimal)response.Temperature,
                until: (DateTime)response.Until
            );

            await statusPublisher.Publish(thermostat, status);
        } catch (Exception ex) { logger.LogError(ex, ex.Message); }
    }
}