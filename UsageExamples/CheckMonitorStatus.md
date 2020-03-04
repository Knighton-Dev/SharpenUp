# Check Monitor Status

This example checks the status of a monitor and prints it's name and status to the console. 

## Code 

```csharp
using System;
using System.Threading.Tasks;
using SharpenUp;
using SharpenUp.Models;
using SharpenUp.Results;

namespace TestApp
{
    class Program
    {
        static async Task Main( string[] args )
        {
            UptimeRobot robot = new UptimeRobot( "YOUR_API_KEY" );

            MonitorsResult monitorsResult = await robot.GetMonitorsAsync();

            if ( monitorsResult.Error == null && monitorsResult.Monitors?.Count > 0 )
            {
                foreach ( Monitor monitor in monitorsResult.Monitors )
                {
                    if ( monitor.Status == MonitorStatus.Down )
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                    }

                    Console.WriteLine( $"{monitor.FriendlyName} is currently {monitor.Status}" );

                    Console.ResetColor();
                }
            }
            else
            {
                if ( monitorsResult.Error != null )
                {
                    Console.WriteLine( $"There was an error: {monitorsResult.Error.Message}" );
                }
                else
                {
                    Console.WriteLine( "No Monitors Found!" );
                }
            }
        }
    }
}
```