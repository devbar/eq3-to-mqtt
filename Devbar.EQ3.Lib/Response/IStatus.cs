using Devbar.EQ3.Lib.Enum;

namespace Devbar.EQ3.Lib.Response;

public interface IStatus
{
    public Mode Mode { get; }
    public ushort Valve { get; }
    public decimal TargetTemperature { get; }
    public DateTime? VacationDate {get;}
    public decimal OpenWindowTemperature {get;}
    public ushort OpenWindowInterval {get;}
    public decimal ComfortTemperature {get;}
    public decimal EcoTemperature {get;}
    public decimal TemperatureOffset {get;}
}