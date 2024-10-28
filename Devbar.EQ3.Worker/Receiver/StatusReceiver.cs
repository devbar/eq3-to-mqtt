using Devbar.EQ3.Worker.Manager;
using Devbar.EQ3.Worker.Publisher;
using RabbitMQ.Client.Events;

namespace Devbar.EQ3.Worker.Receiver;

public class StatusReceiver(
    IConfiguration configuration, 
    IThermostatManager thermostatManager, 
    ILogger<StatusReceiver> logger, 
    IStatusPublisher statusPublisher) : AbstractReceiver(configuration, thermostatManager, logger), IReceiver
{
    private readonly IThermostatManager _thermostatManager = thermostatManager;
    private readonly ILogger _logger = logger;
    protected override string Command => "{0}/{1}/status/command";
    protected override string Tag => "status";

    protected override async Task Receive(object sender, BasicDeliverEventArgs args)
    {
        var thermostat = _thermostatManager.GetThermostat(args.ConsumerTag);

        var status = await thermostat.GetStatusAsync();

        _logger.LogInformation("Receiving: {0}", args.ConsumerTag);

        await statusPublisher.Publish(thermostat, status);
    }
}