using Devbar.EQ3.Lib.Enum;
using Devbar.EQ3.Lib.Exception;
using Devbar.EQ3.Lib.Response;

namespace Devbar.EQ3.Lib.Converter;

public class StatusConverter : IStatusConverter
{
    public IStatus FromBytes(byte[] rawData)
    {
        // 02 01 09 50 04 1e 00 00 00 00 18 03 2a 22 07
        // |  |  |  |  |  |  |  |  |  |  |  |  |  |  + temperature offset, calculate (value -  7) / 2
        // |  |  |  |  |  |  |  |  |  |  |  |  |  + eco temperature, calculate value / 2
        // |  |  |  |  |  |  |  |  |  |  |  |  + comfort temperature, calculate value / 2
        // |  |  |  |  |  |  |  |  |  |  |  + open windows interval, calculate value * 5 minutes
        // |  |  |  |  |  |  |  |  |  |  + temperature in open windows mode, calculate value / 2
        // |  |  |  |  |  |  |  |  |  + if vacation mode: time, calculate value * 30 minutes
        // |  |  |  |  |  |  |  |  + if vacation mode: month (January = 0, February = 1, etc.)
        // |  |  |  |  |  |  |  + if vacation mode: year, callculate value + 2000
        // |  |  |  |  |  |  + if vacation mode: day in month
        // |  |  |  |  |  + target temperature, calculate value / 2
        // |  |  |  |  + (unknown)
        // |  |  |  + Valve in percent 
        // |  |  + Mode, see bits in API for details
        // |  + 0x01 if this notification is device status notification
        // + Always 0x02 if this notification is device status notification

        if (rawData.Length != 15) { throw new ResponseInvalidException($"{nameof(FromBytes)}: The response length does not match"); }

        if (rawData[0] != 0x02 || rawData[1] != 0x01) { throw new ResponseInvalidException($"{nameof(FromBytes)}: The response initialization does not match"); }

        var mode = (Mode)rawData[2];

        var status = new Status(
            mode,
            rawData[3],
            rawData[5] / 2.0m,
            mode == Mode.Vacation
                ? new DateTime
                (
                    rawData[6],
                    rawData[8] + 1,
                    rawData[7] + 2000,
                    rawData[9] / 2,
                    rawData[9] % 2 * 30,
                    0
                )
                : null,
            rawData[10] / 2.0m,
            (ushort)(rawData[11] * 5),
            rawData[12] / 2.0m,
            rawData[13] / 2.0m,
            (rawData[14] - 7m) / 2.0m
        );

        return status;
    }
}