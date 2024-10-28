using Devbar.EQ3.Lib.Converter;
using Devbar.EQ3.Lib.Enum;
using Devbar.EQ3.Lib.Exception;

namespace Devbar.EQ3.Test.Converter;

[TestClass]
public class StatusConverterTest
{
    [TestMethod]
    [TestCategory("unit")]
    public void ShouldConvertFromBytes()
    {
        var statusConverter = new StatusConverter();

        var data = statusConverter.FromBytes([2, 1, 8, 52, 0, 47, 0, 0, 0, 0, 40, 4, 64, 40, 14]);
        
        Assert.AreEqual(Mode.Dst, data.Mode);
        Assert.AreEqual(52, data.Valve);
        Assert.AreEqual(23.5m, data.TargetTemperature);
        Assert.IsNull(data.VacationDate);
        Assert.AreEqual(20m, data.OpenWindowTemperature);
        Assert.AreEqual(20, data.OpenWindowInterval);
        Assert.AreEqual(32, data.ComfortTemperature);
        Assert.AreEqual(20, data.EcoTemperature);
        Assert.AreEqual(3.5m, data.TemperatureOffset);
    }
    
    [DataTestMethod]
    [TestCategory("unit")]
    [DataRow(9, 1, 8, 52, 0, 47, 0, 0, 0, 0, 40, 4, 64, 40, 14)]
    [DataRow(2, 1, 8, 52, 0, 47, 0, 0, 0, 0, 40, 4, 64)]
    [DataRow(1, 1, 8, 52, 0, 47, 0, 0, 0, 0, 40, 4, 64, 0, 0, 0, 0, 0, 0, 0, 0, 0)]
    public void ShouldNotConvertFromBytesAndThrowException(params int[] data)
    {
        // arrange
        var statusConverter = new StatusConverter();
        
        // act, assert
        Assert.ThrowsException<ResponseInvalidException>(() => statusConverter.FromBytes(data.Select(d => (byte)d).ToArray()));
    }
}