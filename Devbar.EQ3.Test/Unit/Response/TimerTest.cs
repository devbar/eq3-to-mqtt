using Timer = Devbar.EQ3.Lib.Response.Timer;

namespace Devbar.EQ3.Test.Response;

[TestClass]
public class TimerTest
{
    [TestMethod]
    public void ShouldCreateDefaultTimer()
    {
        var timer = new Timer(DayOfWeek.Monday);
        
        Assert.AreEqual(0, timer.Event1.Hour);
        Assert.AreEqual(0, timer.Event2.Hour);
        Assert.AreEqual(0, timer.Event3.Hour);
        Assert.AreEqual(0, timer.Event4.Hour);
        Assert.AreEqual(0, timer.Event5.Hour);
        Assert.AreEqual(0, timer.Event6.Hour);
        Assert.AreEqual(0, timer.Event7.Hour);
        
        Assert.AreEqual(0, timer.Event1.Minute);
        Assert.AreEqual(0, timer.Event2.Minute);
        Assert.AreEqual(0, timer.Event3.Minute);
        Assert.AreEqual(0, timer.Event4.Minute);
        Assert.AreEqual(0, timer.Event5.Minute);
        Assert.AreEqual(0, timer.Event6.Minute);
        Assert.AreEqual(0, timer.Event7.Minute);
        
        Assert.AreEqual(0m, timer.Event1.Temperature);
        Assert.AreEqual(0m, timer.Event2.Temperature);
        Assert.AreEqual(0m, timer.Event3.Temperature);
        Assert.AreEqual(0m, timer.Event4.Temperature);
        Assert.AreEqual(0m, timer.Event5.Temperature);
        Assert.AreEqual(0m, timer.Event6.Temperature);
        Assert.AreEqual(0m, timer.Event7.Temperature);
        
        Assert.AreEqual(DayOfWeek.Monday, timer.Day);
    }
}