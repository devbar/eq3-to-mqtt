using Devbar.EQ3.Lib.Response;

namespace Devbar.EQ3.Lib.Converter;

public class EventConverter : IEventConverter
{
    public Event FromBytes(byte temperature, byte time)
    {
        return new Event(temperature / 2m, (ushort)(time / 6), (ushort)(time % 6 * 10));
    }

    public byte[] FromEvent(Event @event)
    {
        var t = new  List<byte> {
            (byte)(@event.Temperature * 2),
            (byte)(@event.Hour * 6 + @event.Minute / 10)
        };

        return t.ToArray();
    }
}