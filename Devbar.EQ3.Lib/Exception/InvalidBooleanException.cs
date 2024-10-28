namespace Devbar.EQ3.Lib.Exception;

public class InvalidBooleanException(string val) : System.Exception($"Invalid boolean value ({val})")
{
}