namespace Devbar.EQ3.Lib.Enum;

public enum Mode : byte
{
    Auto = 0x00,
    Manual = 0x01,
    Vacation = 0x02,
    Boost = 0x04,
    Dst = 0x08,
    OpenWindow = 0x10,
    Locked = 0x20,
    Unknown = 0x40,
    LowBattery = 0x80
}
