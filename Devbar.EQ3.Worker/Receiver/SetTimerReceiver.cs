using System.Data;
using System.Text;
using Devbar.EQ3.Worker.Manager;
using Newtonsoft.Json;
using RabbitMQ.Client.Events;

namespace Devbar.EQ3.Worker.Receiver;

public class SetTimerReceiver(
    IConfiguration configuration,
    IThermostatManager thermostatManager,
    ILogger<StatusReceiver> logger) : AbstractReceiver(configuration, thermostatManager, logger), IReceiver
{
    private readonly IThermostatManager _thermostatManager = thermostatManager;
    protected override string Command => "{0}/{1}/timer/command/set";
    protected override string Tag => "timer-set";

    protected override async Task Receive(object sender, BasicDeliverEventArgs args)
    {
        try {
            var thermostat = _thermostatManager.GetThermostat(args.ConsumerTag);

            var body = Encoding.UTF8.GetString(args.Body.ToArray());

            var timer = JsonConvert.DeserializeObject<Devbar.EQ3.Lib.Response.Timer>(body);

            if (timer == null) { throw new NoNullAllowedException("Sending an empty set timer request is not supported"); }

            await thermostat.SetTimer(timer);
        } catch (Exception ex) { logger.LogError(ex, ex.Message); }
    }
}