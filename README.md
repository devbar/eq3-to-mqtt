# EQ3 to MQTT

This is a dotnet based systemd service to read an control BLE based EQ-3 thermostats.

## Requirements

* Linux
* MQTT server (i.g. RabbitMQ)
* .NET 8 ([instructions to install on your distro](https://learn.microsoft.com/en-us/dotnet/core/install/linux))

This project is based on [Linux.bluetooth](https://github.com/SuessLabs/Linux.Bluetooth) and requires BlueZ v5.50 and above. You can check which version you're using with, `bluetoothd -v`.

Every thermostat you want to control should be paired by using `bluetoothctl`.

All instructions are tested on Raspberry. It should be possible to run it on other systems by changine the publish flags of step 2, but I never tried.  

## Installation

1. Run `git clone https://github.com/devbar/eq3-to-mqtt` to get the latest updates from repo
2. Build the project by calling `dotnet publish Devbar.EQ3.Worker/Devbar.EQ3.Worker.csproj --runtime linux-arm --self-contained --output ~\bin`
3. Go to bin folder with `cd ~\bin`
4. Make binary runable by calling `u+x .\Devbar.EQ3.Worker`
5. Run `.\setup.sh`

## Configuration

Open `/etc/eq3/config.json` with an deditor and change the these lines.

```
{
  "Logging": {
    "LogLevel": {
      "Default": "Debug",
      "Microsoft.Hosting.Lifetime": "Debug"
    }
  },
  "Mqtt": {
    "Hostname": "my.local.mqtt",
    "Port": 5672,
    "VirtualHost": "/",    
    "Username": "",
    "Password": "",
    "Exchange": "amq.topic",
    "RoutingKey": "eq3"
  },
  "Controller": "01:03:03:04:05:06", <-- contoller to use, copy from bluetoothctl
  "Devices": [
    "07:08:09:10:11:12:13", <-- any device you want to control, copy from bluetoothctl
    ...
  ]    
}
```
Restart by calling `systemctl restart eq3-to-mqtt` after changing something.
