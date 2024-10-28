using System.Text;
using Devbar.EQ3.Worker.Manager;
using Devbar.EQ3.Worker.Parser;
using Devbar.EQ3.Worker.Publisher;
using RabbitMQ.Client.Events;

namespace Devbar.EQ3.Worker.Receiver;

public class TimeReceiver(
    IConfiguration configuration,
    IThermostatManager thermostatManager,
    ILogger<StatusReceiver> logger,
    IStatusPublisher statusPublisher,
    IDateTimeParser dateTimeParser) : AbstractReceiver(configuration, thermostatManager, logger), IReceiver
{
    private readonly IThermostatManager _thermostatManager = thermostatManager;
    protected override string Command => "{0}/{1}/time/command";
    protected override string Tag => "time";
    protected override async Task Receive(object sender, BasicDeliverEventArgs args)
    {
        try {
            var thermostat = _thermostatManager.GetThermostat(args.ConsumerTag);

            var body = Encoding.UTF8.GetString(args.Body.ToArray());

            var dateTime = dateTimeParser.Parse(body.Replace("\"", ""));

            var status = await thermostat.SetTimeAsync(dateTime);

            await statusPublisher.Publish(thermostat, status);
        } catch (Exception ex) { logger.LogError(ex, ex.Message); }
    }
}