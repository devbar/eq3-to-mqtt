using Devbar.EQ3.Lib.Exception;

namespace Devbar.EQ3.Worker.Parser;

public class BoolParser : IBoolParser
{
    public bool Parse(string val)
    {
        if (!int.TryParse(val, out var boostInt)) {
            if (bool.TryParse(val, out var boostBool)) { return boostBool; }
        } else {
            switch (boostInt) {
                case 1:
                    return true;
                case 0:
                    return false;
            }
        }

        throw new InvalidBooleanException(val);
    }
}