using System.Text;
using Devbar.EQ3.Worker.Manager;
using Devbar.EQ3.Worker.Publisher;
using Newtonsoft.Json;
using RabbitMQ.Client.Events;

namespace Devbar.EQ3.Worker.Receiver;

public class GetTimerReceiver(
    IConfiguration configuration,
    IThermostatManager thermostatManager,
    ILogger<StatusReceiver> logger,
    ITimerPublisher timerPublisher) : AbstractReceiver(configuration, thermostatManager, logger), IReceiver
{
    private readonly IThermostatManager _thermostatManager = thermostatManager;
    protected override string Command => "{0}/{1}/timer/command/get";
    protected override string Tag => "timer-get";

    protected override async Task Receive(object sender, BasicDeliverEventArgs args)
    {
        try {
            var thermostat = _thermostatManager.GetThermostat(args.ConsumerTag);

            var body = Encoding.UTF8.GetString(args.Body.ToArray());

            var day = JsonConvert.DeserializeObject<DayOfWeek>(body);

            var timer = await thermostat.GetTimer(day);

            await timerPublisher.Publish(thermostat, timer);
        } catch (Exception ex) { logger.LogError(ex, ex.Message); }
    }
}