using Devbar.EQ3.Lib.Converter;

namespace Devbar.EQ3.Test.Converter;

[TestClass]
public class TimeConverterTest
{
    public static IEnumerable<object[]> Dates => [
        [new DateTime(2024, 10, 24, 16, 12, 0), new byte[] { 24, 10, 24, 16, 12, 0 }],
        [new DateTime(2026, 12, 24, 8, 10, 12), new byte[] { 26, 12, 24, 8, 10, 12 }],
    ];

    [DataTestMethod]
    [DynamicData(nameof(Dates))]
    public void ShouldConvertFromDateTime(DateTime data, byte[] expData)
    {
        // arrange
        var timeConverter = new TimeConverter();

        // act
        var time = timeConverter.FromDateTime(data);

        // assert
        for (var i = 0; i < expData.Length; i++) { Assert.AreEqual(expData[i], time[i]); }
    }
}