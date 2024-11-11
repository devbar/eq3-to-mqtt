# EQ3 to MQTT

This is a dotnet based systemd service to read an control BLE based EQ-3 thermostats.

## Requirements

* Linux
* MQTT server (i.g. RabbitMQ)
* .NET 8 ([instructions to install on your distribution](https://learn.microsoft.com/en-us/dotnet/core/install/linux))

This project is based on [Linux.bluetooth](https://github.com/SuessLabs/Linux.Bluetooth) and requires BlueZ v5.50 and above. You can check which version you're using with, `bluetoothd -v`.

Every thermostat you want to control should be paired by using `bluetoothctl`.

All instructions are tested on Raspberry. It should be possible to run it on other systems by changing the publish flags of step 2, but I never tried.  

## Installation

1. Run `git clone https://github.com/devbar/eq3-to-mqtt` to get the latest updates from repo
2. Build the project by calling `dotnet publish Devbar.EQ3.Worker/Devbar.EQ3.Worker.csproj --runtime linux-arm --self-contained --output ~\bin`
3. Go to bin folder with `cd ~\bin`
4. Make binary runnable by calling `u+x .\Devbar.EQ3.Worker`
5. Run `.\setup.sh`

## Configuration

Open `/etc/eq3/config.json` with an editor and change the these lines.

```json
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
  "Controller": "01:03:03:04:05:06", <-- controller to use, copy from bluetoothctl
  "Devices": [
    "07:08:09:10:11:12:13", <-- any device you want to control, copy from bluetoothctl
    ...
  ]    
}
```
Restart by calling `systemctl restart eq3-to-mqtt` after changing something.

## MQTT Endpoints

Default routing key is always ```eq3/eq-3-MACOFDEVICE```. This means, for example, if you want to get the status of device 00:1A:22:11:F3:8B, you can subscribe to ```eq3/eq-3-001A2211F38B/status```.

### Status
```/status/command```
Send an empty message to trigger a pull of status.

```/status```
Receive a json with current settings of the thermostat.

```json
{
  "Mode": 9,
  "Valve": 7,
  "TargetTemperature": 20,
  "VacationDate": null,
  "OpenWindowTemperature": 8,
  "OpenWindowInterval": 10,
  "ComfortTemperature": 23,
  "EcoTemperature": 9,
  "TemperatureOffset": 0
}
```

### Auto
```/auto/command```
Send ```1``` or ```0``` to activate or deactivate the automatic mode. 

The command was successful after a new status with the mode was sent to ```/status```.

### Boost
```/boost/command```
Send ```1``` or ```0``` to activate or deactivate the boost mode. 

The command was successful after a new status with the mode was sent to ```/status```.

### Comfort
```/comfort/command```
Send ```1``` or ```0``` to activate or deactivate the comfort temperature mode. 

The command was successful after a new status with the mode was sent to ```/status```.

### Eco
```/eco/command```
Send ```1``` or ```0``` to activate or deactivate the eco temperature mode. 

The command was successful after a new status with the mode was sent to ```/status```.

### Eco and Comfort Temperature

```/eco_comfort/temperature/command```

Send a json request to change eco and comfort temperature.

```json
{
  "eco": 18.5,
  "comfort": 21.5
}
```
The command was successful after a new status with the mode was sent to ```/status```.

### Timer
```/timer/command/get```

Send a string with a day you want to show.

```json
"monday", "tuesday", ..."
1,2,3,...
```

The command was successful after a new timer was send to ```/timer```.

```/timer/command/set```

Send a json with the events you want to configure.

```json
{
  "Day": 1, /* the day to show */

  "Event1": {
    /* 17C° until 16:30 */
    "Temperature": 17,
    "Hour": 16,
    "Minute": 30
  },
  "Event2": {
    /* 21C° until 22:50 */
    "Temperature": 21,
    "Hour": 22,
    "Minute": 50
  },
  "Event3": {
    "Temperature": 0,
    "Hour": 0,
    "Minute": 0
  },
  "Event4": {
    "Temperature": 0,
    "Hour": 0,
    "Minute": 0
  },
  "Event5": {
    "Temperature": 0,
    "Hour": 0,
    "Minute": 0
  },
  "Event6": {
    "Temperature": 0,
    "Hour": 0,
    "Minute": 0
  },
  "Event7": {
    "Temperature": 0,
    "Hour": 0,
    "Minute": 0
  }
}
```

### Temperature

```/temperature/command```

Send a decimal value (.5 steps) to set the current temperature.

```json
23.5
```

The command was successful after a new status with the mode was sent to ```/status```.

### Time

```/time/command```

Send a ISO timestamp to adjust the thermostat to the correct setting.

```json
2024-11-01T16:44:00
```

### Vacation

```/vacation/command```

Send a json to activate the vacation mode.

```json
{
  "temp": 16.0,
  "until": "2024-11-15T08:00:00"
}
```

The command was successful after a new status with the mode was sent to ```/status```.

### Open Window

```/openwindow/command```

Send a json to activate the open window mode.

```json
{
  "temp": 16:00
  "minutes": 30
}
```

The command was successful after a new status with the mode was sent to ```/status```.


## OpenHAB integration

```js
// Let's say you have a running mqtt broker in OpenHAB

Thing mqtt:topic:eq3_office "EQ3 Office"(mqtt:broker:abc125676) {
    Channels:
        Type number : TargetTemperature     "Zieltemperatur" 
        [
          stateTopic="eq3/eq-3-001AXXXXF38B/status", transformationPattern="JSONPATH:$.TargetTemperature", 
          unit="°C", 
          commandTopic="eq3/eq-3-001A2211F38B/temperature/command"
        ]
        Type switch : Automatic             "Automatik"      
        [
          stateTopic="eq3/eq-3-001AXXXXF38B/status", transformationPattern="JSONPATH:$.Mode∩JS:eq3_mode_to_auto.js", commandTopic="eq3/eq-3-001AXXXXF38B/auto/command", transformationPatternOut="JS:eq3_auto_to_mode.js"
        ]
        Type switch : Boost                 "Boost"          
        [
        stateTopic="eq3/eq-3-001AXXXXF38B/status",   transformationPattern="JSONPATH:$.Mode∩JS:eq3_mode_to_boost.js",   commandTopic="eq3/eq-3-001AXXXXF38B/boost/command",                    transformationPatternOut="JS:eq3_boost_to_mode.js"
        ]
}
```

Some small JS Scripts are necessary to convert data from the queue.

eq3_auto_to_mode.js
```js
(function (i) {
    if(i == "ON")
    {
        return 1
    }
    return 0
})(input)
```
eq3_boost_to_mode.js
```js
(function (i) {
    if(i == "ON")
    {
        return 1
    }
    return 0
})(input)
```
eq3_mode_to_auto.js
```js
(function (i) {
    if(i == 8)
    {
        return "ON"
    }
    return "OFF"
})(input)
```
eq3_mode_to_boost.js
```js
(function (i) {
    if(i == 12)
    {
        return "ON"
    }
    return "OFF"
})(input)
```

## References

* [Einrichten der_Bluetooth Thermostate von eQ-3](https://wiki.fhem.de/wiki/Einrichten_der_Bluetooth-Thermostate_von_eQ-3)

