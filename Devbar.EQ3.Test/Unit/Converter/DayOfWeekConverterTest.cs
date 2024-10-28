using Devbar.EQ3.Lib.Converter;

namespace Devbar.EQ3.Test.Converter;

[TestClass]
public class DayOfWeekConverterTest
{
    [DataTestMethod]
    [DataRow(0, DayOfWeek.Saturday)]
    [DataRow(1, DayOfWeek.Sunday)]
    [DataRow(2, DayOfWeek.Monday)]
    [DataRow(3, DayOfWeek.Tuesday)]
    [DataRow(4, DayOfWeek.Wednesday)]
    [DataRow(5, DayOfWeek.Thursday)]
    [DataRow(6, DayOfWeek.Friday)]
    public void ShouldConvertByteToDayOfWeek(int value, DayOfWeek expDayOfWeek)
    {
        // arrange
        var dayOfWeekConverter = new DayOfWeekConverter();

        // act
        var result = dayOfWeekConverter.FromByte((byte)value);

        // assert
        Assert.AreEqual(expDayOfWeek, result);
    }
    
    [DataTestMethod]
    [DataRow(8)]
    [DataRow(255)]
    public void ShouldNotConvertByteAndThrowException(int value)
    {
        // arrange
        var dayOfWeekConverter = new DayOfWeekConverter();

        // act, assert
        Assert.ThrowsException<ArgumentOutOfRangeException>(() => dayOfWeekConverter.FromByte((byte)value));
    }

    [DataTestMethod]
    [DataRow(DayOfWeek.Saturday, 0)]
    [DataRow(DayOfWeek.Sunday, 1)]
    [DataRow(DayOfWeek.Monday, 2)]
    [DataRow(DayOfWeek.Tuesday, 3)]
    [DataRow(DayOfWeek.Wednesday, 4)]
    [DataRow(DayOfWeek.Thursday, 5)]
    [DataRow(DayOfWeek.Friday, 6)]
    public void ShouldConvertDayOfWeekToByte(DayOfWeek dayOfWeek, int expectedByte)
    {
        // arrange
        var dayOfWeekConverter = new DayOfWeekConverter();

        // act
        var result = dayOfWeekConverter.FromDayOfWeek(dayOfWeek);

        // assert
        Assert.AreEqual(expectedByte, result);
    }
    
    [DataTestMethod]
    [DataRow(8)]
    [DataRow(255)]
    [DataRow(-1)]
    public void ShouldNotConvertDayOfWeekAndThrowException(DayOfWeek dayOfWeek)
    {
        // arrange
        var dayOfWeekConverter = new DayOfWeekConverter();

        // act, assert
        Assert.ThrowsException<ArgumentOutOfRangeException>(() => dayOfWeekConverter.FromDayOfWeek(dayOfWeek));
    }
}