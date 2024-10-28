using Devbar.EQ3.Lib.Converter;
using Devbar.EQ3.Lib.Response;

namespace Devbar.EQ3.Test.Converter;

[TestClass]
public class EventConverterTest
{
    [DataTestMethod]
    [TestCategory("unit")]
    [DataRow(43, 15, 21.5, 2, 30)]
    [DataRow(36, 136, 18, 22, 40)]
    [DataRow(46, 36, 23, 6, 0)]
    public void ShouldConvertBytesToEvent(int temperature, int time, double expTemp, int expHour, int expMinute)
    {
        // arrange
        var converter = new EventConverter();
        
        // act
        var @event = converter.FromBytes((byte)temperature, (byte)time);
        
        // assert
        Assert.AreEqual((decimal)expTemp, @event.Temperature);
        Assert.AreEqual(expHour, @event.Hour);
        Assert.AreEqual(expMinute, @event.Minute);
    }
    
    public static IEnumerable<object[]> Events => [
        [new Event(21.5m, 2,30), 43, 15],
        [new Event(18m, 22,40), 36, 136],
        [new Event(23m, 6,0), 46, 36]
    ];

    [TestMethod]
    [DynamicData(nameof(Events))]
    [TestCategory("unit")]
    public void ShouldConvertEventsToBytes(Event @event, int expTemperature, int expTime)
    {
        // arrange
        var converter = new EventConverter();
        
        // act
        var bytes = converter.FromEvent(@event);

        // assert
        Assert.AreEqual(bytes[0], expTemperature);
        Assert.AreEqual(bytes[1], expTime);
    }
}