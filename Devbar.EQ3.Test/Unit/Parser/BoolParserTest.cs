using Devbar.EQ3.Worker.Parser;

namespace Devbar.EQ3.Test.Parser;

[TestClass]
public class BoolParserTest
{
    [DataTestMethod]
    [DataRow("1")]
    [DataRow("true")]
    public void ShouldParseForTrue(string val)
    {
        // arrange
        var boolParser = new BoolParser();
        
        // act, assert
        Assert.IsTrue(boolParser.Parse(val));
    }

    [DataTestMethod]
    [DataRow("0")]
    [DataRow("false")]
    public void ShouldParseForFalse(string val)
    {
        var boolParser = new BoolParser();
        
        // act, assert
        Assert.IsFalse(boolParser.Parse(val));
    }
}