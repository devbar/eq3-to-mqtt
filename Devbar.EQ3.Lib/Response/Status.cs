using Devbar.EQ3.Lib.Enum;

namespace Devbar.EQ3.Lib.Response;

public class Status(
    Mode mode, 
    ushort valve, 
    decimal targetTemperature, 
    DateTime? vacationDate, 
    decimal openWindowTemperature,
    ushort openWindowInterval, 
    decimal comfortTemperature,
    decimal ecoTemperature,
    decimal temperatureOffset
) : IStatus
{
    public Mode Mode { get; } = mode;
    public ushort Valve { get; } = valve;
    public decimal TargetTemperature { get; } = targetTemperature;
    public DateTime? VacationDate { get; } = vacationDate;
    public decimal OpenWindowTemperature {get; } = openWindowTemperature;
    public ushort OpenWindowInterval {get; } = openWindowInterval;
    public decimal ComfortTemperature {get; } = comfortTemperature;
    public decimal EcoTemperature {get; } = ecoTemperature;
    public decimal TemperatureOffset {get; } = temperatureOffset;
}