using RabbitMQ.Client;
using Devbar.EQ3.Worker.Manager;
using Devbar.EQ3.Worker.Receiver;

namespace Devbar.EQ3.Worker;

public class Worker(
    IConnection connection,
    IThermostatManager thermostatManager,
    IEnumerable<IReceiver> receiver
) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var model = connection.CreateModel();

        foreach (var r in receiver) { await r.Register(model); }

        while (!stoppingToken.IsCancellationRequested) { await Task.Delay(10000, stoppingToken); }

        foreach (var r in receiver) { r.UnRegister(); }
        
        foreach (var thermostat in thermostatManager.GetAll()) { model.BasicCancel(await thermostat.GetNameAsync()); }
    }
}