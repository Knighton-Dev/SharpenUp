using System;
using System.Threading.Tasks;
using SharpenUp.Models;
using SharpenUp.Requests;
using SharpenUp.Results;
using Xunit;
using System.Collections.Generic;

namespace SharpenUp.Tests
{
    public class UptimeRobotTests
    {
        private readonly UptimeRobot _goodRobot;
        private readonly UptimeRobot _badRobot;

        public UptimeRobotTests()
        {
            _goodRobot = new UptimeRobot( Environment.GetEnvironmentVariable( "GOOD_API_KEY" ) );
            _badRobot = new UptimeRobot( "badKey" );
        }

        #region GetAccountDetails

        [Fact]
        public async Task GetAccountDetailsAsync()
        {
            AccountDetailsResult result = await _goodRobot.GetAccountDetailsAsync();

            Assert.Equal( Status.ok, result.Status );
            Assert.Null( result.Error );
            Assert.Equal( Environment.GetEnvironmentVariable( "ACCOUNT_EMAIL" ), result.Account.Email );
        }

        [Fact]
        public async Task GetAccountDetailsAsync_BadKey()
        {
            AccountDetailsResult result = await _badRobot.GetAccountDetailsAsync();

            Assert.Equal( Status.fail, result.Status );
            Assert.Null( result.Account );
            Assert.Equal( "invalid_parameter", result.Error.Type );
            Assert.Equal( "api_key", result.Error.ParameterName );
            Assert.Equal( "badKey", result.Error.PassedValue );
            Assert.Equal( "api_key is invalid.", result.Error.Message );
        }

        #endregion

        #region Monitors

        [Fact]
        public async Task GetMonitorsAsync_NoRequest()
        {
            MonitorsResult result = await _goodRobot.GetMonitorsAsync();

            Assert.Equal( Status.ok, result.Status );
            Assert.Null( result.Error );
            Assert.NotNull( result.Monitors );
            Assert.Equal( 0, result.Pagination.Offset );
            Assert.Equal( 50, result.Pagination.Limit );
            Assert.True( result.Pagination.Total > 0 );
        }

        [Fact]
        public async Task GetMonitorsAsync_WithRequest()
        {
            int monitorOne = Convert.ToInt32( Environment.GetEnvironmentVariable( "GOOD_MONITOR_ID_1" ) );
            int monitorTwo = Convert.ToInt32( Environment.GetEnvironmentVariable( "GOOD_MONITOR_ID_2" ) );

            MonitorsRequest request = new MonitorsRequest
            {
                Monitors = new List<int> { monitorOne, monitorTwo },
                MonitorTypes = new List<MonitorType> { MonitorType.Keyword, MonitorType.HTTP },
                Statuses = new List<MonitorStatus> { MonitorStatus.Up },
                CustomUptimeRatios = new List<int> { 3, 5, 15 }
            };

            MonitorsResult result = await _goodRobot.GetMonitorsAsync( request );

            Assert.Equal( monitorOne, result.Monitors[ 0 ].Id );
            Assert.Equal( Environment.GetEnvironmentVariable( "GOOD_MONITOR_1_FRIENDLY_NAME" ), result.Monitors[ 0 ].FriendlyName );
            Assert.Equal( Environment.GetEnvironmentVariable( "GOOD_MONITOR_1_URL" ), result.Monitors[ 0 ].URL );
            Assert.Equal( MonitorType.Keyword, result.Monitors[ 0 ].MonitorType );
            // TODO: Test SubType
            // TODO: Test KeywordType
            Assert.Equal( "OK", result.Monitors[ 0 ].KeywordValue );
            Assert.True( string.IsNullOrEmpty( result.Monitors[ 0 ].HttpUsername ) );
            Assert.True( string.IsNullOrEmpty( result.Monitors[ 0 ].HttpPassword ) );
            // TODO: Test Port
            Assert.Equal( 300, result.Monitors[ 0 ].Interval );
            Assert.True( string.IsNullOrEmpty( result.Monitors[ 0 ].CustomHttpHeaders ) );
            Assert.True( string.IsNullOrEmpty( result.Monitors[ 0 ].CustomHttpStatuses ) );
        }

        #endregion

        #region Alert Contacts

        [Fact]
        public async Task GetAlertContactsAsync_NoRequest()
        {
            AlertContactsResult result = await _goodRobot.GetAlertContactsAsync();

            Assert.Equal( Status.ok, result.Status );
            Assert.Null( result.Error );
            Assert.NotNull( result.AlertContacts );
            Assert.Equal( 0, result.Offset );
            Assert.Equal( 50, result.Limit );
            Assert.True( result.Total > 0 );
        }

        [Fact]
        public async Task GetAlertContactsAsync_WithRequest()
        {
            AlertContactsRequest request = new AlertContactsRequest
            {
                Offset = 2,
                Limit = 20
            };

            AlertContactsResult result = await _goodRobot.GetAlertContactsAsync( request );

            Assert.Equal( Status.ok, result.Status );
            Assert.Null( result.Error );
            Assert.NotNull( result.AlertContacts );
            Assert.Equal( 2, result.Offset );
            Assert.Equal( 20, result.Limit );
            Assert.True( result.Total > 0 );
        }

        #endregion

        #region Maintenance Windows

        #endregion

        #region Public Status Pages

        [Fact]
        public async Task GetPublicStatusPagesAsync_NoRequest()
        {
            PublicStatusPageResult result = await _goodRobot.GetPublicStatusPagesAsync();

            Assert.Equal( Status.ok, result.Status );
            Assert.Null( result.Error );
            Assert.NotNull( result.PublicStatusPages );
            Assert.Equal( 0, result.Pagination.Offset );
            Assert.Equal( 50, result.Pagination.Limit );
            Assert.True( result.Pagination.Total > 0 );
        }

        [Fact]
        public async Task GetPublicStatusPagesAsync_WithRequest()
        {
            PublicStatusPageRequest request = new PublicStatusPageRequest
            {
                Offset = 1,
                Limit = 30
            };

            PublicStatusPageResult result = await _goodRobot.GetPublicStatusPagesAsync( request );

            Assert.Equal( Status.ok, result.Status );
            Assert.Null( result.Error );
            Assert.NotNull( result.PublicStatusPages );
            Assert.Equal( 1, result.Pagination.Offset );
            Assert.Equal( 30, result.Pagination.Limit );
            Assert.True( result.Pagination.Total > 0 );
        }

        #endregion
    }
}
