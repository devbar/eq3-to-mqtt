using Devbar.EQ3.Worker.Parser;

namespace Devbar.EQ3.Test.Parser;

[TestClass]
public class DateTimeParserTest
{
    public static IEnumerable<object[]> Dates => [
        ["2024-10-14T13:00", new DateTime(2024, 10, 14, 13, 00, 00)],
    ];
    
    [DataTestMethod]
    [DynamicData(nameof(Dates))]
    public void ShouldParseDateTime(string dateTime, DateTime expected)
    {
        var parser = new DateTimeParser();

        Assert.AreEqual(expected, parser.Parse(dateTime));
    }

}