using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
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
        private readonly IConfiguration _config;

        public MonitorsTests()
        {
            _config = new ConfigurationBuilder().SetBasePath( Directory.GetCurrentDirectory() ).AddJsonFile( "appsettings.json", false, true ).Build();
            _goodManager = new UptimeManager( _config[ "GOOD_API_KEY" ] );
            _badManager = new UptimeManager( "thisKeyIsBad" );
        }

        [Fact]
        public async Task Monitor_GoodKey_GoodId()
        {
            MonitorsResult result = await _goodManager.GetMonitorAsync( Convert.ToInt32( _config[ "GOOD_MONITOR_ID_1" ] ) );

            Assert.True( result.Status == RequestStatusType.ok );
            Assert.True( result.Error == null );
            Assert.True( result.Results[ 0 ].FriendlyName == _config[ "GOOD_MONITOR_1_FRIENDLY_NAME" ] );
        }

        [Fact]
        public async Task Monitors_GoodKey_GoodIds()
        {
            List<int> monitorIds = new List<int> {
                Convert.ToInt32( _config[ "GOOD_MONITOR_ID_1" ] ),
                Convert.ToInt32( _config[ "GOOD_MONITOR_ID_2" ] ) };

            MonitorsResult result = await _goodManager.GetMonitorsAsync( monitorIds );

            Assert.True( result.Status == RequestStatusType.ok );
            Assert.True( result.Error == null );
            Assert.True( result.Results[ 0 ].FriendlyName == _config[ "GOOD_MONITOR_1_FRIENDLY_NAME" ] );
            Assert.True( result.Results[ 1 ].FriendlyName == _config[ "GOOD_MONITOR_2_FRIENDLY_NAME" ] );
        }

        [Fact]
        public async Task AllMonitors_GoodKey()
        {
            MonitorsResult result = await _goodManager.GetMonitorsAsync();

            Assert.True( result.Status == RequestStatusType.ok );
            Assert.True( result.Error == null );
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
