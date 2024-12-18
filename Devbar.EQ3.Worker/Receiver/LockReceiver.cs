using System.Text;
using Devbar.EQ3.Lib.Exception;
using Devbar.EQ3.Lib.Response;
using Devbar.EQ3.Worker.Manager;
using Devbar.EQ3.Worker.Parser;
using Devbar.EQ3.Worker.Publisher;
using RabbitMQ.Client.Events;

namespace Devbar.EQ3.Worker.Receiver;

public class LockReceiver(
    IConfiguration configuration,
    IThermostatManager thermostatManager,
    ILogger<StatusReceiver> logger,
    IStatusPublisher statusPublisher,
    IBoolParser boolParser) : AbstractReceiver(configuration, thermostatManager, logger), IReceiver
{
    private readonly IThermostatManager _thermostatManager = thermostatManager;
    protected override string Command => "{0}/{1}/lock/command";
    protected override string Tag => "lock";
    protected override async Task Receive(object sender, BasicDeliverEventArgs args)
    {
        try {
            var thermostat = _thermostatManager.GetThermostat(args.ConsumerTag);

            var body = Encoding.UTF8.GetString(args.Body.ToArray());

            IStatus status;

            if (boolParser.Parse(body)) { status = await thermostat.LockAsync(); } else { status = await thermostat.UnLockAsync(); }

            await statusPublisher.Publish(thermostat, status);
        } catch (Exception ex) { logger.LogError(ex, ex.Message); }
    }
}