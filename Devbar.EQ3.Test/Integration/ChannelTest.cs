
using Devbar.EQ3.Lib.Manager;

[TestClass]
public class ChannelTest
{
    private readonly Channel _channel = new ("00:1A:22:11:F3:8B", new DeviceManager("00:1A:7D:DA:71:10"));

    public ChannelTest(){
        _channel.ValueChanged += (s, b) => {
            Console.Write("LOG: ");
            foreach(var k in b){
                Console.Write(k +",");
            }
            Console.WriteLine();
        };
    }

    [TestMethod]
    [TestCategory("integration")]
    public async Task ShouldGetStatus()
    {
        var status = await _channel.GetStatusAsync();

        foreach(var s in status)
        {
            Console.Write(s  + ",");
        }
    }

    [TestMethod]
    [TestCategory("integration")]
    public async Task ShouldGetStatusInALoop()
    {
        for(int i = 0; i < 10; i++)
        {
            var status = await _channel.GetStatusAsync();

            Console.Write("LOOP #{0}:", i);

            foreach(var s in status)
            {
                Console.Write(s  + ",");
            }

            Console.WriteLine();
            await Task.Delay(2000);
        }        
    }

    [TestMethod]
    [TestCategory("integration")]
    public async Task ShouldGetName()
    {
        var status = await _channel.GetNameAsync();

        foreach(var s in status)
        {
            Console.Write(s  + ",");
        }
    }

    [TestMethod]
    [TestCategory("integration")]
    public async Task ShouldStartAndStopBoost()    
    {
        var status = await _channel.StartBoostAsync();

        Console.Write("Start: ");
        foreach(var s in status)
        {
            Console.Write(s  + ",");
        }
        Console.WriteLine();

        await Task.Delay(20000);

        status = await _channel.StopBoostAsync();

        Console.Write("Stop: ");
        foreach(var s in status)
        {
            Console.Write(s  + ",");
        }
    }

    [TestMethod]
    [TestCategory("integration")]
    public async Task ShouldSetTime()    
    {
        var now = DateTime.UtcNow;        

        var status = await _channel.SetTimeAsync([             
            (byte)(now.Year % 100), 
            (byte)now.Month, 
            (byte)now.Day, 
            (byte)now.Hour, 
            (byte)now.Minute, 
            (byte)now.Second 
        ]);
        
        foreach(var s in status)
        {
            Console.Write(s  + ",");
        }        
    }

    [TestMethod]
    [TestCategory("integration")]
    public async Task ShouldSetTemperature()    
    {
        var status = await _channel.SetTemperatureAsync(0x2d);

        foreach(var s in status)
        {
            Console.Write(s  + ",");
        }        
    }

    [TestMethod]
    [TestCategory("integration")]
    public async Task ShouldGetTimer()    
    {
        var status = await _channel.GetTimer(0x02);
        
        foreach(var s in status)
        {
            Console.Write(s  + ",");
        }        
    }

    [TestMethod]
    [TestCategory("integration")]
    public async Task ShouldSetTimer()    
    {
        var status = await _channel.SetTimer(0x02, [0x22, 0x63, 0x2a, 0x89, 0x22, 0x90, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00]);                                          
        
        foreach(var s in status)
        {
            Console.Write(s  + ",");
        }        
    }

    [TestMethod]
    [TestCategory("integration")]
    public async Task ShouldSwitchToComfortTemperature()    
    {
        var status = await _channel.SwitchToComfortTemperature();
        
        foreach(var s in status)
        {
            Console.Write(s  + ",");
        } 
    }

    [TestMethod]
    [TestCategory("integration")]
    public async Task ShouldSwitchToEcoTemperature()    
    {
        var status = await _channel.SwitchToEcoTemperature();
        
        foreach(var s in status)
        {
            Console.Write(s  + ",");
        }        
    }

    [TestMethod]
    [TestCategory("integration")]
    public async Task ShouldSetEcoAndComfortTemp()    
    {
        var status = await _channel.SetEcoAndComfortTemperature(0x2e, 0x12);
        
        foreach(var s in status)
        {
            Console.Write(s  + ",");
        }        
    }

    [TestMethod]
    [TestCategory("integration")]
    public async Task ShouldSetWindowOpenMode()    
    {
        var status = await _channel.SetWindowOpenMode(0x10, 0x12);
        
        foreach(var s in status)
        {
            Console.Write(s  + ",");
        }        
    }

    [TestMethod]
    [TestCategory("integration")]
    public async Task ShouldSwitchToAutoMode()    
    {
        var status = await _channel.SwitchToAutoMode();
        
        foreach(var s in status)
        {
            Console.Write(s  + ",");
        }        
    }

    [TestMethod]
    [TestCategory("integration")]
    public async Task ShouldSwitchToManualMode()    
    {
        var status = await _channel.SwitchToManualMode();
        
        foreach(var s in status)
        {
            Console.Write(s  + ",");
        }        
    }

    [TestMethod]
    [TestCategory("integration")]
    public async Task ShouldSetVacationMode()    
    {
        var status = await _channel.SetVacationMode(0x10, 12, 24, 0x2b, 9);
        
        foreach(var s in status)
        {
            Console.Write(s  + ",");
        }        
    }
}