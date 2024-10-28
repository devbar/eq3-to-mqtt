using Devbar.EQ3.Lib.Response;

namespace Devbar.EQ3.Lib.Converter;

public interface IStatusConverter
{
    IStatus FromBytes(byte[] data);
}