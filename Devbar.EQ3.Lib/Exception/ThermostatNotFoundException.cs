namespace Devbar.EQ3.Lib.Exception;

public class ThermostatNotFoundException(string thermostat): System.Exception($"Thermostat {thermostat} was not found.")
{
}