using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SharpenUp.Client;
using SharpenUp.Common;
using SharpenUp.Common.Models.Monitors;
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

        #region Test Single Monitor ID

        [Fact]
        public async Task SingleMonitor_GoodKey_GoodId()
        {
            MonitorsRequest request = new MonitorsRequest
            {
                MonitorIds = new List<int> { Convert.ToInt32( Environment.GetEnvironmentVariable( "GOOD_MONITOR_ID_1" ) ) }
            };

            // This is kind of weird logic, but I wanted to make sure I was getting a DateTime from the result,not an int.
            DateTimeOffset offset = DateTimeOffset.FromUnixTimeSeconds( 1564749285 );
            DateTime createDate = offset.UtcDateTime;

            MonitorsResult result = await _goodManager.GetMonitorsAsync( request );

            Assert.Equal( RequestStatusType.ok, result.Status );
            Assert.Null( result.Error );
            Assert.Equal( Environment.GetEnvironmentVariable( "GOOD_MONITOR_1_FRIENDLY_NAME" ), result.Results[ 0 ].FriendlyName );
            Assert.Null( result.Results[ 0 ].Logs );
            Assert.Equal( createDate, result.Results[ 0 ].CreationDate );
        }

        #endregion

        #region Test Single Flags

        [Fact]
        public async Task SingleMonitor_GoodKey_GoodId_WithMonitorTypes()
        {
            MonitorsRequest request = new MonitorsRequest
            {
                MonitorIds = new List<int> { Convert.ToInt32( Environment.GetEnvironmentVariable( "GOOD_MONITOR_ID_1" ) ) },
                MonitorTypes = new List<MonitorType> { MonitorType.Https, MonitorType.Keyword }
            };

            MonitorsResult result = await _goodManager.GetMonitorsAsync( request );

            Assert.Equal( RequestStatusType.ok, result.Status );
            Assert.Null( result.Error );
            Assert.Equal( Environment.GetEnvironmentVariable( "GOOD_MONITOR_1_FRIENDLY_NAME" ), result.Results[ 0 ].FriendlyName );
            Assert.NotEqual( MonitorType.Heartbeat, result.Results[ 0 ].MonitorType );
        }

        [Fact]
        public async Task SingleMonitor_GoodKey_GoodId_WithUptimeDateRanges()
        {
            MonitorsRequest request = new MonitorsRequest
            {
                MonitorIds = new List<int> { Convert.ToInt32( Environment.GetEnvironmentVariable( "GOOD_MONITOR_ID_1" ) ) },
                UptimeDateRanges = new List<Tuple<DateTime, DateTime>>
                {
                    new Tuple<DateTime, DateTime>( new DateTime( 2019, 8, 2, 12, 49, 30 ), new DateTime( 2019, 8, 14, 14, 36, 10 ) )
                }
            };

            MonitorsResult result = await _goodManager.GetMonitorsAsync( request );

            Assert.Equal( RequestStatusType.ok, result.Status );
            Assert.Null( result.Error );
            Assert.Equal( Environment.GetEnvironmentVariable( "GOOD_MONITOR_1_FRIENDLY_NAME" ), result.Results[ 0 ].FriendlyName );
            Assert.Equal( 100.00, result.Results[ 0 ].CustomUptimeRatio );
        }

        [Fact]
        public async Task SingleMonitor_GoodKey_GoodId_WithUptimeDurations()
        {
            MonitorsRequest request = new MonitorsRequest
            {
                MonitorIds = new List<int> { Convert.ToInt32( Environment.GetEnvironmentVariable( "GOOD_MONITOR_ID_1" ) ) },
                IncludeAllTimeUptimeDurations = true
            };

            MonitorsResult result = await _goodManager.GetMonitorsAsync( request );

            Assert.Equal( RequestStatusType.ok, result.Status );
            Assert.Null( result.Error );
            Assert.Equal( Environment.GetEnvironmentVariable( "GOOD_MONITOR_1_FRIENDLY_NAME" ), result.Results[ 0 ].FriendlyName );
            Assert.Equal( 0, result.Results[ 0 ].UptimeDuration.Paused );
        }

        [Fact]
        public async Task SingleMonitor_GoodKey_GoodId_WithLogs()
        {
            MonitorsRequest request = new MonitorsRequest
            {
                MonitorIds = new List<int> { Convert.ToInt32( Environment.GetEnvironmentVariable( "GOOD_MONITOR_ID_1" ) ) },
                IncludeLogs = true
            };

            MonitorsResult result = await _goodManager.GetMonitorsAsync( request );

            Assert.Equal( RequestStatusType.ok, result.Status );
            Assert.Null( result.Error );
            Assert.Equal( Environment.GetEnvironmentVariable( "GOOD_MONITOR_1_FRIENDLY_NAME" ), result.Results[ 0 ].FriendlyName );
            Assert.NotNull( result.Results[ 0 ].Logs );
        }

        [Fact]
        public async Task SingleMonitor_GoodKey_GoodId_WithLogs_WithStartDate()
        {
            DateTime startDate = new DateTime( 2020, 1, 25, 10, 00, 00 );

            MonitorsRequest request = new MonitorsRequest
            {
                MonitorIds = new List<int> { Convert.ToInt32( Environment.GetEnvironmentVariable( "GOOD_MONITOR_ID_2" ) ) },
                IncludeLogs = true,
                LogsStartDate = startDate
            };

            MonitorsResult result = await _goodManager.GetMonitorsAsync( request );

            Assert.Equal( RequestStatusType.ok, result.Status );
            Assert.Null( result.Error );
            Assert.Equal( Environment.GetEnvironmentVariable( "GOOD_MONITOR_2_FRIENDLY_NAME" ), result.Results[ 0 ].FriendlyName );
            Assert.NotNull( result.Results[ 0 ].Logs );
            Assert.True( result.Results[ 0 ].Logs[ result.Results[ 0 ].Logs.Count - 1 ].IncidentTime > startDate );
        }

        [Fact]
        public async Task SingleMonitor_GoodKey_GoodId_WithLogs_WithEndDate()
        {
            DateTime endDate = new DateTime( 2020, 1, 25, 10, 00, 00 );

            MonitorsRequest request = new MonitorsRequest
            {
                MonitorIds = new List<int> { Convert.ToInt32( Environment.GetEnvironmentVariable( "GOOD_MONITOR_ID_2" ) ) },
                IncludeLogs = true,
                LogsEndDate = endDate
            };

            MonitorsResult result = await _goodManager.GetMonitorsAsync( request );

            Assert.Equal( RequestStatusType.ok, result.Status );
            Assert.Null( result.Error );
            Assert.Equal( Environment.GetEnvironmentVariable( "GOOD_MONITOR_2_FRIENDLY_NAME" ), result.Results[ 0 ].FriendlyName );
            Assert.NotNull( result.Results[ 0 ].Logs );
            Assert.True( result.Results[ 0 ].Logs[ 0 ].IncidentTime < endDate );
        }

        [Fact]
        public async Task SingleMonitor_GoodKey_GoodId_WithLogs_WithBothDates()
        {
            DateTime startDate = new DateTime( 2020, 1, 25, 10, 00, 00 );
            DateTime endDate = new DateTime( 2020, 2, 15, 10, 00, 00 );

            MonitorsRequest request = new MonitorsRequest
            {
                MonitorIds = new List<int> { Convert.ToInt32( Environment.GetEnvironmentVariable( "GOOD_MONITOR_ID_2" ) ) },
                IncludeLogs = true,
                LogsStartDate = startDate,
                LogsEndDate = endDate
            };

            MonitorsResult result = await _goodManager.GetMonitorsAsync( request );

            Assert.Equal( RequestStatusType.ok, result.Status );
            Assert.Null( result.Error );
            Assert.Equal( Environment.GetEnvironmentVariable( "GOOD_MONITOR_2_FRIENDLY_NAME" ), result.Results[ 0 ].FriendlyName );
            Assert.NotNull( result.Results[ 0 ].Logs );
            Assert.InRange( result.Results[ 0 ].Logs[ 0 ].IncidentTime, startDate, endDate );
            Assert.InRange( result.Results[ 0 ].Logs[ result.Results[ 0 ].Logs.Count - 1 ].IncidentTime, startDate, endDate );
        }

        [Fact]
        public async Task SingleMonitor_GoodKey_GoodId_WithLogs_WithLimit()
        {
            MonitorsRequest request = new MonitorsRequest
            {
                MonitorIds = new List<int> { Convert.ToInt32( Environment.GetEnvironmentVariable( "GOOD_MONITOR_ID_1" ) ) },
                IncludeLogs = true,
                LogsLimit = 5
            };

            MonitorsResult result = await _goodManager.GetMonitorsAsync( request );

            Assert.Equal( RequestStatusType.ok, result.Status );
            Assert.Null( result.Error );
            Assert.Equal( Environment.GetEnvironmentVariable( "GOOD_MONITOR_1_FRIENDLY_NAME" ), result.Results[ 0 ].FriendlyName );
            Assert.NotNull( result.Results[ 0 ].Logs );
            Assert.Equal( 5, result.Results[ 0 ].Logs.Count );
        }

        [Fact]
        public async Task SingleMonitor_GoodKey_GoodId_WithResponseTimes()
        {
            MonitorsRequest request = new MonitorsRequest
            {
                MonitorIds = new List<int> { Convert.ToInt32( Environment.GetEnvironmentVariable( "GOOD_MONITOR_ID_1" ) ) },
                IncludeResponseTimes = true
            };

            MonitorsResult result = await _goodManager.GetMonitorsAsync( request );

            Assert.Equal( RequestStatusType.ok, result.Status );
            Assert.Null( result.Error );
            Assert.Equal( Environment.GetEnvironmentVariable( "GOOD_MONITOR_1_FRIENDLY_NAME" ), result.Results[ 0 ].FriendlyName );
            Assert.NotNull( result.Results[ 0 ].ResponseTimes );
        }

        [Fact( Skip = "Need to sort out the converter." )]
        public async Task SingleMonitor_GoodKey_GoodId_WithResponseTimes_WithBothDates()
        {
            DateTime startDate = DateTime.Now.AddDays( -1 );
            DateTime endDate = DateTime.Now;

            MonitorsRequest request = new MonitorsRequest
            {
                MonitorIds = new List<int> { Convert.ToInt32( Environment.GetEnvironmentVariable( "GOOD_MONITOR_ID_1" ) ) },
                IncludeResponseTimes = true,
                ResponseTimesStartDate = startDate,
                ResponseTimesEndDate = endDate
            };

            MonitorsResult result = await _goodManager.GetMonitorsAsync( request );

            Assert.Equal( RequestStatusType.ok, result.Status );
            Assert.Null( result.Error );
            Assert.Equal( Environment.GetEnvironmentVariable( "GOOD_MONITOR_1_FRIENDLY_NAME" ), result.Results[ 0 ].FriendlyName );
            Assert.NotNull( result.Results[ 0 ].ResponseTimes );
        }

        [Fact]
        public async Task SingleMonitor_GoodKey_GoodId_WithUptime()
        {
            MonitorsRequest request = new MonitorsRequest
            {
                MonitorIds = new List<int> { Convert.ToInt32( Environment.GetEnvironmentVariable( "GOOD_MONITOR_ID_1" ) ) },
                IncludeAllTimeUptimeRatio = true
            };

            MonitorsResult result = await _goodManager.GetMonitorsAsync( request );

            Assert.Equal( RequestStatusType.ok, result.Status );
            Assert.Null( result.Error );
            Assert.Equal( Environment.GetEnvironmentVariable( "GOOD_MONITOR_1_FRIENDLY_NAME" ), result.Results[ 0 ].FriendlyName );
            Assert.NotEqual( 0, result.Results[ 0 ].UptimeRatio );
        }

        [Fact]
        public async Task SingleMonitor_GoodKey_GoodId_WithTimezone()
        {
            MonitorsRequest request = new MonitorsRequest
            {
                MonitorIds = new List<int> { Convert.ToInt32( Environment.GetEnvironmentVariable( "GOOD_MONITOR_ID_1" ) ) },
                IncludeTimezone = true
            };

            MonitorsResult result = await _goodManager.GetMonitorsAsync( request );

            Assert.Equal( RequestStatusType.ok, result.Status );
            Assert.Null( result.Error );
            Assert.Equal( Environment.GetEnvironmentVariable( "GOOD_MONITOR_1_FRIENDLY_NAME" ), result.Results[ 0 ].FriendlyName );
            Assert.NotEqual( 0, result.TimeZone );
        }

        [Fact]
        public async Task AllMonitors_GoodKey_GoodId_WithStatusTypes()
        {
            MonitorsRequest request = new MonitorsRequest
            {
                StatusTypes = new List<OnlineStatusType> { OnlineStatusType.Online }
            };

            MonitorsResult result = await _goodManager.GetMonitorsAsync( request );

            Assert.Equal( RequestStatusType.ok, result.Status );
            Assert.Null( result.Error );
            Assert.Equal( Environment.GetEnvironmentVariable( "GOOD_MONITOR_1_FRIENDLY_NAME" ), result.Results[ 0 ].FriendlyName );
            Assert.NotEqual( OnlineStatusType.Paused, result.Results[ 0 ].OnlineStatus );
        }

        [Fact]
        public async Task AllMonitors_GoodKey_WithLogs()
        {
            MonitorsRequest request = new MonitorsRequest
            {
                IncludeLogs = true
            };

            MonitorsResult result = await _goodManager.GetMonitorsAsync( request );

            Assert.Equal( RequestStatusType.ok, result.Status );
            Assert.Null( result.Error );
            Assert.NotNull( result.Results[ 0 ].Logs );
        }

        [Fact]
        public async Task AllMonitors_GoodKey_WithUptime()
        {
            MonitorsRequest request = new MonitorsRequest
            {
                IncludeAllTimeUptimeRatio = true
            };

            MonitorsResult result = await _goodManager.GetMonitorsAsync( request );

            Assert.Equal( RequestStatusType.ok, result.Status );
            Assert.Null( result.Error );
            Assert.NotEqual( 0, result.Results[ 0 ].UptimeRatio );
        }

        [Fact]
        public async Task AllMonitors_GoodKey_WithTimezone()
        {
            MonitorsRequest request = new MonitorsRequest
            {
                IncludeTimezone = true
            };

            MonitorsResult result = await _goodManager.GetMonitorsAsync( request );

            Assert.Equal( RequestStatusType.ok, result.Status );
            Assert.Null( result.Error );
            Assert.NotEqual( 0, result.TimeZone );
        }

        [Fact]
        public async Task MultipleMonitors_GoodKey_GoodIds_WithLogs()
        {
            MonitorsRequest request = new MonitorsRequest
            {
                MonitorIds = new List<int> {
                    Convert.ToInt32( Environment.GetEnvironmentVariable( "GOOD_MONITOR_ID_1" ) ),
                    Convert.ToInt32( Environment.GetEnvironmentVariable( "GOOD_MONITOR_ID_2" ) ) },
                IncludeLogs = true
            };

            MonitorsResult result = await _goodManager.GetMonitorsAsync( request );

            Assert.Equal( RequestStatusType.ok, result.Status );
            Assert.Null( result.Error );
            Assert.Equal( Environment.GetEnvironmentVariable( "GOOD_MONITOR_1_FRIENDLY_NAME" ), result.Results[ 0 ].FriendlyName );
            Assert.Equal( Environment.GetEnvironmentVariable( "GOOD_MONITOR_2_FRIENDLY_NAME" ), result.Results[ 1 ].FriendlyName );
            Assert.NotNull( result.Results[ 0 ].Logs );
            Assert.NotNull( result.Results[ 1 ].Logs );
        }

        [Fact]
        public async Task MultipleMonitors_GoodKey_GoodIds_WithUptime()
        {
            MonitorsRequest request = new MonitorsRequest
            {
                MonitorIds = new List<int> {
                    Convert.ToInt32( Environment.GetEnvironmentVariable( "GOOD_MONITOR_ID_1" ) ),
                    Convert.ToInt32( Environment.GetEnvironmentVariable( "GOOD_MONITOR_ID_2" ) ) },
                IncludeAllTimeUptimeRatio = true
            };

            MonitorsResult result = await _goodManager.GetMonitorsAsync( request );

            Assert.Equal( RequestStatusType.ok, result.Status );
            Assert.Null( result.Error );
            Assert.Equal( Environment.GetEnvironmentVariable( "GOOD_MONITOR_1_FRIENDLY_NAME" ), result.Results[ 0 ].FriendlyName );
            Assert.Equal( Environment.GetEnvironmentVariable( "GOOD_MONITOR_2_FRIENDLY_NAME" ), result.Results[ 1 ].FriendlyName );
            Assert.NotEqual( 0, result.Results[ 0 ].UptimeRatio );
            Assert.NotEqual( 0, result.Results[ 1 ].UptimeRatio );
        }

        [Fact]
        public async Task MultipleMonitors_GoodKey_GoodIds_WithTimezone()
        {
            MonitorsRequest request = new MonitorsRequest
            {
                MonitorIds = new List<int> {
                    Convert.ToInt32( Environment.GetEnvironmentVariable( "GOOD_MONITOR_ID_1" ) ),
                    Convert.ToInt32( Environment.GetEnvironmentVariable( "GOOD_MONITOR_ID_2" ) ) },
                IncludeTimezone = true
            };

            MonitorsResult result = await _goodManager.GetMonitorsAsync( request );

            Assert.Equal( RequestStatusType.ok, result.Status );
            Assert.Null( result.Error );
            Assert.Equal( Environment.GetEnvironmentVariable( "GOOD_MONITOR_1_FRIENDLY_NAME" ), result.Results[ 0 ].FriendlyName );
            Assert.Equal( Environment.GetEnvironmentVariable( "GOOD_MONITOR_2_FRIENDLY_NAME" ), result.Results[ 1 ].FriendlyName );
            Assert.NotEqual( 0, result.TimeZone );
            Assert.NotEqual( 0, result.TimeZone );
        }

        #endregion

        #region Test Combinations of Flags

        [Fact]
        public async Task SingleMonitor_GoodKey_GoodId_WithLogs_WithUptime()
        {
            MonitorsRequest request = new MonitorsRequest
            {
                MonitorIds = new List<int> { Convert.ToInt32( Environment.GetEnvironmentVariable( "GOOD_MONITOR_ID_1" ) ) },
                IncludeLogs = true,
                IncludeAllTimeUptimeRatio = true
            };

            MonitorsResult result = await _goodManager.GetMonitorsAsync( request );

            Assert.Equal( RequestStatusType.ok, result.Status );
            Assert.Null( result.Error );
            Assert.Equal( Environment.GetEnvironmentVariable( "GOOD_MONITOR_1_FRIENDLY_NAME" ), result.Results[ 0 ].FriendlyName );
            Assert.NotNull( result.Results[ 0 ].Logs );
            Assert.NotEqual( 0, result.Results[ 0 ].UptimeRatio );
        }

        [Fact]
        public async Task SingleMonitor_GoodKey_GoodId_WithLogs_WithUptime_WithAlertContact()
        {
            MonitorsRequest request = new MonitorsRequest
            {
                MonitorIds = new List<int> { Convert.ToInt32( Environment.GetEnvironmentVariable( "GOOD_MONITOR_ID_1" ) ) },
                IncludeLogs = true,
                IncludeAllTimeUptimeRatio = true,
                IncludeAlertContacts = true
            };

            MonitorsResult result = await _goodManager.GetMonitorsAsync( request );

            Assert.Equal( RequestStatusType.ok, result.Status );
            Assert.Null( result.Error );
            Assert.Equal( Environment.GetEnvironmentVariable( "GOOD_MONITOR_1_FRIENDLY_NAME" ), result.Results[ 0 ].FriendlyName );
            Assert.NotNull( result.Results[ 0 ].Logs );
            Assert.NotEqual( 0, result.Results[ 0 ].UptimeRatio );
            Assert.NotNull( result.Results[ 0 ].AlertContacts );
            Assert.Equal( Environment.GetEnvironmentVariable( "ACCOUNT_EMAIL" ), result.Results[ 0 ].AlertContacts[ 0 ].Value );
        }

        [Fact]
        public async Task SingleMonitor_GoodKey_GoodId_WithLogs_WithUptime_WithAlertContact_WithSSL()
        {
            MonitorsRequest request = new MonitorsRequest
            {
                MonitorIds = new List<int> { Convert.ToInt32( Environment.GetEnvironmentVariable( "GOOD_MONITOR_ID_1" ) ) },
                IncludeLogs = true,
                IncludeAllTimeUptimeRatio = true,
                IncludeAlertContacts = true,
                IncludeSSLInfo = true
            };

            MonitorsResult result = await _goodManager.GetMonitorsAsync( request );

            Assert.Equal( RequestStatusType.ok, result.Status );
            Assert.Null( result.Error );
            Assert.Equal( Environment.GetEnvironmentVariable( "GOOD_MONITOR_1_FRIENDLY_NAME" ), result.Results[ 0 ].FriendlyName );
            Assert.NotNull( result.Results[ 0 ].Logs );
            Assert.NotEqual( 0, result.Results[ 0 ].UptimeRatio );
            Assert.NotNull( result.Results[ 0 ].AlertContacts );
            Assert.NotNull( result.Results[ 0 ].SSLInfo );
        }

        [Fact]
        public async Task SingleMonitor_GoodKey_GoodId_WithLogs_WithUptime_WithAlertContact_WithSSL_WithTimezone()
        {
            MonitorsRequest request = new MonitorsRequest
            {
                MonitorIds = new List<int> { Convert.ToInt32( Environment.GetEnvironmentVariable( "GOOD_MONITOR_ID_1" ) ) },
                IncludeLogs = true,
                IncludeAllTimeUptimeRatio = true,
                IncludeAlertContacts = true,
                IncludeSSLInfo = true,
                IncludeTimezone = true
            };

            MonitorsResult result = await _goodManager.GetMonitorsAsync( request );

            Assert.Equal( RequestStatusType.ok, result.Status );
            Assert.Null( result.Error );
            Assert.Equal( Environment.GetEnvironmentVariable( "GOOD_MONITOR_1_FRIENDLY_NAME" ), result.Results[ 0 ].FriendlyName );
            Assert.NotNull( result.Results[ 0 ].Logs );
            Assert.NotEqual( 0, result.Results[ 0 ].UptimeRatio );
            Assert.NotNull( result.Results[ 0 ].AlertContacts );
            Assert.NotNull( result.Results[ 0 ].SSLInfo );
            Assert.NotEqual( 0, result.TimeZone );
        }

        #endregion

        #region Test for Multiple Monitor IDs

        [Fact]
        public async Task MultipleMonitors_GoodKey_GoodIds()
        {
            MonitorsRequest request = new MonitorsRequest
            {
                MonitorIds = new List<int> {
                    Convert.ToInt32( Environment.GetEnvironmentVariable( "GOOD_MONITOR_ID_1" ) ),
                    Convert.ToInt32( Environment.GetEnvironmentVariable( "GOOD_MONITOR_ID_2" ) ) }
            };

            MonitorsResult result = await _goodManager.GetMonitorsAsync( request );

            Assert.Equal( RequestStatusType.ok, result.Status );
            Assert.NotNull( result.Results );
            Assert.Equal( Environment.GetEnvironmentVariable( "GOOD_MONITOR_1_FRIENDLY_NAME" ), result.Results[ 0 ].FriendlyName );
            Assert.Equal( Environment.GetEnvironmentVariable( "GOOD_MONITOR_2_FRIENDLY_NAME" ), result.Results[ 1 ].FriendlyName );
        }

        #endregion

        #region Test with No IDs

        [Fact]
        public async Task AllMonitors_GoodKey()
        {
            MonitorsResult result = await _goodManager.GetMonitorsAsync( new MonitorsRequest() );

            Assert.Equal( RequestStatusType.ok, result.Status );
            Assert.NotNull( result.Results );
            Assert.Equal( 0, result.Pagination.Offset );
            Assert.Equal( 50, result.Pagination.Limit );
            Assert.Equal( 7, result.Pagination.Total );
        }

        [Fact]
        public async Task AllMonitors_GoodKey_NoRequest()
        {
            MonitorsResult result = await _goodManager.GetMonitorsAsync();

            Assert.Equal( RequestStatusType.ok, result.Status );
            Assert.Null( result.Error );
        }

        #endregion

        [Fact]
        public async Task AllMonitors_BadKey()
        {
            MonitorsResult result = await _badManager.GetMonitorsAsync( new MonitorsRequest() );

            Assert.Equal( RequestStatusType.fail, result.Status );
            Assert.Null( result.Results );
        }
    }
}
