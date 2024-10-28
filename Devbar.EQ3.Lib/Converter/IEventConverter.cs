using Devbar.EQ3.Lib.Response;

namespace Devbar.EQ3.Lib.Converter;

public interface IEventConverter
{
    Event FromBytes(byte temperature, byte time);
    byte[] FromEvent(Event @event);
}