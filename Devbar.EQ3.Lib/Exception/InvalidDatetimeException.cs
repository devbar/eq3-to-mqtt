namespace Devbar.EQ3.Lib.Exception;

public class InvalidDatetimeException(string val) : System.Exception($"Invalid iso timestamp value ({val})")
{
}