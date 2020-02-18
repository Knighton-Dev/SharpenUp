﻿using System;
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
            MonitorsResult result = await _goodManager.GetMonitorAsync( Convert.ToInt32( Environment.GetEnvironmentVariable( "GOOD_MONITOR_ID_1" ) ) );

            if ( !result.Error.Type.Equals( "internal" ) )
            {
                Assert.True( result.Status == RequestStatusType.ok );
                Assert.True( result.Error == null );
                Assert.True( result.Results[ 0 ].FriendlyName == Environment.GetEnvironmentVariable( "GOOD_MONITOR_1_FRIENDLY_NAME" ) );
                Assert.Null( result.Results[ 0 ].Logs );
            }
        }

        [Fact]
        public async Task Monitor_GoodKey_GoodId_WithLogs()
        {
            MonitorsResult result = await _goodManager.GetMonitorAsync( Convert.ToInt32( Environment.GetEnvironmentVariable( "GOOD_MONITOR_ID_1" ) ), true );

            if ( !result.Error.Type.Equals( "internal" ) )
            {
                Assert.True( result.Status == RequestStatusType.ok );
                Assert.True( result.Error == null );
                Assert.True( result.Results[ 0 ].FriendlyName == Environment.GetEnvironmentVariable( "GOOD_MONITOR_1_FRIENDLY_NAME" ) );
                Assert.NotNull( result.Results[ 0 ].Logs );
            }
        }

        [Fact]
        public async Task Monitor_GoodKey_GoodId_WithLogs_WithUptime()
        {
            MonitorsResult result = await _goodManager.GetMonitorAsync( Convert.ToInt32( Environment.GetEnvironmentVariable( "GOOD_MONITOR_ID_1" ) ), true, true );


            if ( !result.Error.Type.Equals( "internal" ) )
            {
                Assert.True( result.Status == RequestStatusType.ok );
                Assert.True( result.Error == null );
                Assert.True( result.Results[ 0 ].FriendlyName == Environment.GetEnvironmentVariable( "GOOD_MONITOR_1_FRIENDLY_NAME" ) );
                Assert.NotNull( result.Results[ 0 ].Logs );
                Assert.True( result.Results[ 0 ].UptimeRatio != 0 );
            }
        }

        [Fact]
        public async Task Monitors_GoodKey_GoodIds()
        {
            List<int> monitorIds = new List<int> {
                Convert.ToInt32( Environment.GetEnvironmentVariable( "GOOD_MONITOR_ID_1" ) ),
                Convert.ToInt32( Environment.GetEnvironmentVariable( "GOOD_MONITOR_ID_2" ) ) };

            MonitorsResult result = await _goodManager.GetMonitorsAsync( monitorIds );

            if ( !result.Error.Type.Equals( "internal" ) )
            {
                Assert.True( result.Status == RequestStatusType.ok );
                Assert.True( result.Error == null );
                Assert.True( result.Results[ 0 ].FriendlyName == Environment.GetEnvironmentVariable( "GOOD_MONITOR_1_FRIENDLY_NAME" ) );
                Assert.True( result.Results[ 1 ].FriendlyName == Environment.GetEnvironmentVariable( "GOOD_MONITOR_2_FRIENDLY_NAME" ) );
                Assert.Null( result.Results[ 0 ].Logs );
                Assert.Null( result.Results[ 1 ].Logs );
            }
        }

        [Fact]
        public async Task Monitors_GoodKey_GoodIds_WithLogs()
        {
            List<int> monitorIds = new List<int> {
                Convert.ToInt32( Environment.GetEnvironmentVariable( "GOOD_MONITOR_ID_1" ) ),
                Convert.ToInt32( Environment.GetEnvironmentVariable( "GOOD_MONITOR_ID_2" ) ) };

            MonitorsResult result = await _goodManager.GetMonitorsAsync( monitorIds, true );


            if ( !result.Error.Type.Equals( "internal" ) )
            {
                Assert.True( result.Status == RequestStatusType.ok );
                Assert.True( result.Error == null );
                Assert.True( result.Results[ 0 ].FriendlyName == Environment.GetEnvironmentVariable( "GOOD_MONITOR_1_FRIENDLY_NAME" ) );
                Assert.True( result.Results[ 1 ].FriendlyName == Environment.GetEnvironmentVariable( "GOOD_MONITOR_2_FRIENDLY_NAME" ) );
                Assert.NotNull( result.Results[ 0 ].Logs );
                Assert.NotNull( result.Results[ 1 ].Logs );
            }
        }

        [Fact]
        public async Task AllMonitors_GoodKey()
        {
            MonitorsResult result = await _goodManager.GetMonitorsAsync();

            if ( !result.Error.Type.Equals( "internal" ) )
            {
                Assert.True( result.Status == RequestStatusType.ok );
                Assert.True( result.Error == null );
                Assert.Null( result.Results[ 0 ].Logs );
            }
        }

        [Fact]
        public async Task AllMonitors_GoodKey_WithLogs()
        {
            MonitorsResult result = await _goodManager.GetMonitorsAsync( true );

            if ( !result.Error.Type.Equals( "internal" ) )
            {
                Assert.True( result.Status == RequestStatusType.ok );
                Assert.True( result.Error == null );
                Assert.NotNull( result.Results[ 0 ].Logs );
            }
        }

        [Fact]
        public async Task AllMonitors_BadKey()
        {
            MonitorsResult result = await _badManager.GetMonitorsAsync();

            Assert.True( result.Status == RequestStatusType.fail );
            Assert.True( result.Results == null );
        }
    }
}
