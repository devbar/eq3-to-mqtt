using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Devbar.EQ3.Worker.Receiver;

public interface IReceiver
{
    Task<AsyncEventingBasicConsumer> Register(IModel model);
    void UnRegister();
}