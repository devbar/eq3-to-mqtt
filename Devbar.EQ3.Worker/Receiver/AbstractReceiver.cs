using Devbar.EQ3.Worker.Manager;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Devbar.EQ3.Worker.Receiver;

public abstract class AbstractReceiver(IConfiguration configuration, IThermostatManager thermostatManager, ILogger logger)
{
    private readonly string _exchange = configuration.GetValue<string>("Mqtt:Exchange") ?? throw new Exception("Add a Mqtt:Exchange for the configuration");
    private readonly string _routingKey = configuration.GetValue<string>("Mqtt:RoutingKey") ?? throw new Exception("Add a Mqtt:RoutingKey for the configuration");
    
    private AsyncEventingBasicConsumer? _consumer;
    protected abstract string Command { get;}
    protected abstract string Tag { get;}
    protected abstract Task Receive(object sender, BasicDeliverEventArgs args);
    
    public async Task<AsyncEventingBasicConsumer> Register(
        IModel model)
    {
        _consumer = new AsyncEventingBasicConsumer(model);
        _consumer.Received += Receive;

        foreach (var thermostat in thermostatManager.GetAll()) {
            var key = string.Format(Command, _routingKey, await thermostat.GetNameAsync());
            
            var queue = key.Replace("/", "-");

            logger.LogDebug("Creating queue {0}", queue);
            
            model.QueueDeclare(queue);
            model.QueueBind(queue, exchange: _exchange, routingKey: key.Replace("/", "."));
            model.BasicConsume(
                queue,
                true,
                await thermostat.GetNameAsync() + $"_{Tag}",
                _consumer);

            logger.LogInformation("Listening: {0}", queue);
        }

        return _consumer;
    }

    public void UnRegister()
    {
        if (_consumer != null) { _consumer.Received -= Receive; }
    }
}