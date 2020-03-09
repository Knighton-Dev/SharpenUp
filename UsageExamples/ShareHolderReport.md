# Share Holder Report

This is an example of a report I use to provide my supervisor. It calculates uptime for the previous month, current month, last seven days, and a YTD. It is somewhat customized and could be cleaned up, but I thought it was a really good "practical" example of using the library to collate and combine data in ways that may not be available through the Uptime Robot website. 

## Code

```csharp
using System;
using System.Collections.Generic;
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

            List<int> productionMonitors = new List<int> { 1234, 5678, 9012 }; // These are not real ID's. 
            DateTime reportDate = new DateTime( 2020, 3, 1 ); // This is the only date that needs changed from month to month. 

            Tuple<DateTime, DateTime> thisCycle = new Tuple<DateTime, DateTime>( reportDate.AddMonths( -1 ), reportDate );
            Tuple<DateTime, DateTime> lastSevenDays = new Tuple<DateTime, DateTime>( reportDate.AddDays( -7 ), reportDate );
            Tuple<DateTime, DateTime> previousCycle = new Tuple<DateTime, DateTime>( reportDate.AddMonths( -2 ), reportDate.AddMonths( -1 ) );
            Tuple<DateTime, DateTime> thisYear = new Tuple<DateTime, DateTime>( new DateTime( 2020, 1, 1 ), reportDate );

            MonitorsRequest request = new MonitorsRequest
            {
                Monitors = productionMonitors,
                CustomUptimeRanges = new List<Tuple<DateTime, DateTime>> { thisCycle, lastSevenDays, previousCycle, thisYear }
            };

            MonitorsResult monthResult = await robot.GetMonitorsAsync( request );

            double overallUptimeSum = 0.0;

            if ( monthResult.Monitors?.Count > 0 )
            {
                foreach ( Monitor monitor in monthResult.Monitors )
                {
                    overallUptimeAverage += monitor.CustomUptimeRanges[ 3 ];
                    Console.WriteLine( $"{ monitor.FriendlyName} : This Cycle {monitor.CustomUptimeRanges[ 0 ]} | 7 Day Uptime {monitor.CustomUptimeRanges[ 1 ]} | Previous Cycle {monitor.CustomUptimeRanges[ 2 ]}" );
                }

                Console.WriteLine( $"Year to Date Average: {overallUptimeSum / monthResult.Monitors.Count}" );
            }
        }
    }
}
```
