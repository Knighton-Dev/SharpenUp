using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SharpenUp.Client;
using SharpenUp.Common;
using SharpenUp.Common.Models;
using SharpenUp.Common.Types;
using Xunit;

namespace SharpenUp.Tests
{
    public class MonitorsTests
    {
        private readonly IUptimeManager _goodManager;
        private readonly IUptimeManager _badManager;

        public MonitorsTests()
        {
            _goodManager = new UptimeManager( Environment.GetEnvironmentVariable( "GOOD_API_KEY" ) );
            _badManager = new UptimeManager( "thisKeyIsBad" );
        }

        [Fact]
        public async Task Monitor_GoodKey_GoodId()
        {
            MonitorsRequest request = new MonitorsRequest
            {
                MonitorIds = new List<int> { Convert.ToInt32( Environment.GetEnvironmentVariable( "GOOD_MONITOR_ID_1" ) ) }
            };

            MonitorsResult result = await _goodManager.GetMonitorsAsync( request );

            Assert.True( result.Status == RequestStatusType.ok );
            Assert.True( result.Error == null );
            Assert.True( result.Results[ 0 ].FriendlyName == Environment.GetEnvironmentVariable( "GOOD_MONITOR_1_FRIENDLY_NAME" ) );
            Assert.Null( result.Results[ 0 ].Logs );
        }

        [Fact]
        public async Task Monitor_GoodKey_GoodId_WithLogs()
        {
            MonitorsRequest request = new MonitorsRequest
            {
                MonitorIds = new List<int> { Convert.ToInt32( Environment.GetEnvironmentVariable( "GOOD_MONITOR_ID_1" ) ) },
                IncludeLogs = true
            };

            MonitorsResult result = await _goodManager.GetMonitorsAsync( request );

            Assert.True( result.Status == RequestStatusType.ok );
            Assert.True( result.Error == null );
            Assert.True( result.Results[ 0 ].FriendlyName == Environment.GetEnvironmentVariable( "GOOD_MONITOR_1_FRIENDLY_NAME" ) );
            Assert.NotNull( result.Results[ 0 ].Logs );
        }

        [Fact]
        public async Task Monitor_GoodKey_GoodId_WithLogs_WithUptime()
        {
            MonitorsRequest request = new MonitorsRequest
            {
                MonitorIds = new List<int> { Convert.ToInt32( Environment.GetEnvironmentVariable( "GOOD_MONITOR_ID_1" ) ) },
                IncludeLogs = true,
                IncludeAllTimeUptimeRatio = true
            };

            MonitorsResult result = await _goodManager.GetMonitorsAsync( request );

            Assert.True( result.Status == RequestStatusType.ok );
            Assert.True( result.Error == null );
            Assert.True( result.Results[ 0 ].FriendlyName == Environment.GetEnvironmentVariable( "GOOD_MONITOR_1_FRIENDLY_NAME" ) );
            Assert.NotNull( result.Results[ 0 ].Logs );
            Assert.True( result.Results[ 0 ].UptimeRatio != 0 );
        }

        [Fact]
        public async Task Monitors_GoodKey_GoodIds()
        {
            MonitorsRequest request = new MonitorsRequest
            {
                MonitorIds = new List<int> {
                    Convert.ToInt32( Environment.GetEnvironmentVariable( "GOOD_MONITOR_ID_1" ) ),
                    Convert.ToInt32( Environment.GetEnvironmentVariable( "GOOD_MONITOR_ID_2" ) ) }
            };

            MonitorsResult result = await _goodManager.GetMonitorsAsync( request );

            Assert.True( result.Status == RequestStatusType.ok );
            Assert.True( result.Error == null );
            Assert.True( result.Results[ 0 ].FriendlyName == Environment.GetEnvironmentVariable( "GOOD_MONITOR_1_FRIENDLY_NAME" ) );
            Assert.True( result.Results[ 1 ].FriendlyName == Environment.GetEnvironmentVariable( "GOOD_MONITOR_2_FRIENDLY_NAME" ) );
            Assert.Null( result.Results[ 0 ].Logs );
            Assert.Null( result.Results[ 1 ].Logs );
        }

        [Fact]
        public async Task Monitors_GoodKey_GoodIds_WithLogs()
        {
            MonitorsRequest request = new MonitorsRequest
            {
                MonitorIds = new List<int> {
                    Convert.ToInt32( Environment.GetEnvironmentVariable( "GOOD_MONITOR_ID_1" ) ),
                    Convert.ToInt32( Environment.GetEnvironmentVariable( "GOOD_MONITOR_ID_2" ) ) },
                IncludeLogs = true
            };

            MonitorsResult result = await _goodManager.GetMonitorsAsync( request );

            Assert.True( result.Status == RequestStatusType.ok );
            Assert.True( result.Error == null );
            Assert.True( result.Results[ 0 ].FriendlyName == Environment.GetEnvironmentVariable( "GOOD_MONITOR_1_FRIENDLY_NAME" ) );
            Assert.True( result.Results[ 1 ].FriendlyName == Environment.GetEnvironmentVariable( "GOOD_MONITOR_2_FRIENDLY_NAME" ) );
            Assert.NotNull( result.Results[ 0 ].Logs );
            Assert.NotNull( result.Results[ 1 ].Logs );
        }

        [Fact]
        public async Task AllMonitors_GoodKey()
        {
            MonitorsResult result = await _goodManager.GetMonitorsAsync( new MonitorsRequest() );

            Assert.True( result.Status == RequestStatusType.ok );
            Assert.True( result.Error == null );
            Assert.Null( result.Results[ 0 ].Logs );
        }

        [Fact]
        public async Task AllMonitors_GoodKey_WithLogs()
        {
            MonitorsRequest request = new MonitorsRequest
            {
                IncludeLogs = true
            };

            MonitorsResult result = await _goodManager.GetMonitorsAsync( request );

            Assert.True( result.Status == RequestStatusType.ok );
            Assert.True( result.Error == null );
            Assert.NotNull( result.Results[ 0 ].Logs );
        }

        [Fact]
        public async Task AllMonitors_BadKey()
        {
            MonitorsResult result = await _badManager.GetMonitorsAsync( new MonitorsRequest() );

            Assert.True( result.Status == RequestStatusType.fail );
            Assert.True( result.Results == null );
        }
    }
}
