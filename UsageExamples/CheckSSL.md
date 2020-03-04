# Check SSL Information

This example checks the expiration date for the SSL Certificates on URL's being monitored and writes a message if the Expiration Date is within a certain number of days. 

## Code

```csharp
using System;
using System.Threading.Tasks;
using SharpenUp;
using SharpenUp.Models;
using SharpenUp.Requests;
using SharpenUp.Results;

namespace TestApp
{
    class Program
    {
        static async Task Main( string[] args )
        {
            UptimeRobot robot = new UptimeRobot( "YOUR_API_KEY" );

            MonitorsRequest monitorsRequest = new MonitorsRequest
            {
                IncludeSSL = true
            };

            MonitorsResult monitorsResult = await robot.GetMonitorsAsync( monitorsRequest );

            if ( monitorsResult.Error == null && monitorsResult.Monitors?.Count > 0 )
            {
                foreach ( Monitor monitor in monitorsResult.Monitors )
                {
                    if ( monitor.MonitorType == MonitorType.Keyword && monitorsRequest.IncludeSSL )
                    {
                        TimeSpan timeToExpiration = monitor.SSL.Expires - DateTime.UtcNow;
                        if ( timeToExpiration.TotalDays < 100 )
                        {
                            Console.WriteLine( $"The SSL Certificate for {monitor.FriendlyName} expires on {monitor.SSL.Expires.ToShortDateString()}." );
                        }
                    }
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