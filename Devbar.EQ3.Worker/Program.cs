using Devbar.EQ3.Lib.Converter;
using Devbar.EQ3.Lib.Manager;
using Devbar.EQ3.Worker;
using Devbar.EQ3.Worker.Manager;
using Devbar.EQ3.Worker.Parser;
using Devbar.EQ3.Worker.Publisher;
using Devbar.EQ3.Worker.Receiver;
using RabbitMQ.Client;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddHostedService<Worker>();
builder.Services.AddTransient((s) =>
{
    var factory = new ConnectionFactory {
        DispatchConsumersAsync = true
    };

    var mqtt = builder.Configuration.GetSection("Mqtt");

    factory.UserName = mqtt.GetValue<string>("Username") ?? throw new Exception("Add a Mqtt:Username for the configuration");
    factory.Password = mqtt.GetValue<string>("Password") ?? throw new Exception("Add a Mqtt:Password for the configuration");
    factory.VirtualHost = mqtt.GetValue<string>("VirtualHost") ?? throw new Exception("Add a Mqtt:VirtualHost for the configuration");
    factory.HostName = mqtt.GetValue<string>("Hostname") ?? throw new Exception("Add a Mqtt:Hostname for the configuration");
    factory.Port = mqtt.GetValue<int?>("Port") ?? throw new Exception("Add a Mqtt:Port for the configuration");

    return factory.CreateConnection();
});
builder.Services.AddSingleton<IDeviceManager>((s) => {
    var controllerAddress = builder.Configuration.GetValue<string>("controller") ?? throw new Exception("Add a controller for the configuration");

    return new DeviceManager(controllerAddress);
});

builder.Services.AddTransient<IEventConverter, EventConverter>();
builder.Services.AddTransient<IDayOfWeekConverter, DayOfWeekConverter>();
builder.Services.AddTransient<ITimeConverter, TimeConverter>();
builder.Services.AddTransient<IStatusConverter, StatusConverter>();

builder.Services.AddTransient<IStatusPublisher, StatusPublisher>();
builder.Services.AddTransient<ITimerPublisher, TimerPublisher>();

builder.Services.AddTransient<IBoolParser, BoolParser>();
builder.Services.AddTransient<IDateTimeParser, DateTimeParser>();

builder.Services.AddTransient<IReceiver, StatusReceiver>();
builder.Services.AddTransient<IReceiver, BoostReceiver>();
builder.Services.AddTransient<IReceiver, TimeReceiver>();
builder.Services.AddTransient<IReceiver, TemperatureReceiver>();
builder.Services.AddTransient<IReceiver, AutoReceiver>();
builder.Services.AddTransient<IReceiver, ComfortReceiver>();
builder.Services.AddTransient<IReceiver, EcoComfortTemperatureReceiver>();
builder.Services.AddTransient<IReceiver, EcoReceiver>();
builder.Services.AddTransient<IReceiver, GetTimerReceiver>();
builder.Services.AddTransient<IReceiver, LockReceiver>();
builder.Services.AddTransient<IReceiver, OpenWindowReceiver>();
builder.Services.AddTransient<IReceiver, SetTimerReceiver>();
builder.Services.AddTransient<IReceiver, VacationReceiver>();

builder.Services.AddSingleton<IThermostatManager>((s) => {
    var devices = builder.Configuration.GetSection("Devices").Get<string[]>() ?? throw new Exception("Add Devices for the configuration");

    return new ThermostatManager(
        devices,
        s.GetRequiredService<IDeviceManager>(),
        s.GetRequiredService<IEventConverter>(),
        s.GetRequiredService<IDayOfWeekConverter>(),
        s.GetRequiredService<IStatusConverter>(),
        s.GetRequiredService<ITimeConverter>());
});

builder.Configuration.AddJsonFile("/etc/eq3/config.json", true, false);

var host = builder.Build();
host.Run();
