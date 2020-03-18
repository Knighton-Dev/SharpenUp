using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SharpenUp.Models;
using SharpenUp.Requests;
using SharpenUp.Results;
using Xunit;
using System.Linq;

namespace SharpenUp.Tests
{
    public class UptimeRobotTests
    {
        private readonly UptimeRobot _goodRobot;
        private readonly UptimeRobot _badRobot;
        private readonly string _defaultExplanation = "There was an error in processing your request. Please see the message.";

        public UptimeRobotTests()
        {
            _goodRobot = new UptimeRobot( Environment.GetEnvironmentVariable( "GOOD_API_KEY" ) );
            _badRobot = new UptimeRobot( "badKey" );
        }

        #region GetAccountDetails

        [Fact]
        public async Task GetAccountDetails()
        {
            AccountDetailsResult result = await _goodRobot.GetAccountDetailsAsync();

            // Status
            Assert.Equal( Status.ok, result.Status );

            // Error
            Assert.Null( result.Error );

            // Pagination
            Assert.Null( result.Pagination );

            // Account
            Assert.True( !string.IsNullOrWhiteSpace( result.Account.Email ) );
            Assert.True( result.Account.MonitorLimit.HasValue );
            Assert.True( result.Account.MonitorInterval.HasValue );
            Assert.True( result.Account.UpMonitors.HasValue );
            Assert.True( result.Account.DownMonitors.HasValue );
            Assert.True( result.Account.PausedMonitors.HasValue );
        }

        [Fact]
        public async Task GetAccountDetails_BadKey()
        {
            AccountDetailsResult result = await _badRobot.GetAccountDetailsAsync();

            // Status
            Assert.Equal( Status.fail, result.Status );

            // Error
            Assert.Equal( "invalid_parameter", result.Error.Explanation );
            Assert.Equal( "api_key", result.Error.ParameterName );
            Assert.Equal( "badKey", result.Error.PassedValue );
            Assert.Equal( "api_key is invalid.", result.Error.Message );

            // Pagination
            Assert.Null( result.Pagination );

            // Account
            Assert.Null( result.Account );
        }

        #endregion

        #region Monitors

        [Fact]
        public async Task GetMonitors()
        {
            MonitorsResult result = await _goodRobot.GetMonitorsAsync();

            // Status
            Assert.Equal( Status.ok, result.Status );

            // Limit
            Assert.NotNull( result.Pagination.Limit );

            // Offset
            Assert.NotNull( result.Pagination.Offset );

            // Total
            Assert.NotNull( result.Pagination.Total );

            // Base Monitor
            Assert.Null( result.BaseMonitor );

            // Monitors
            Monitor sampleMonitor = result.Monitors.FirstOrDefault();

            Assert.NotNull( sampleMonitor.Id );
            Assert.False( string.IsNullOrWhiteSpace( sampleMonitor.FriendlyName ) );
            Assert.False( string.IsNullOrWhiteSpace( sampleMonitor.URL ) );
            Assert.NotNull( sampleMonitor.MonitorType );
            if ( sampleMonitor.MonitorType == MonitorType.Keyword )
            {
                Assert.False( string.IsNullOrWhiteSpace( sampleMonitor.KeywordValue ) );
            }
            Assert.True( string.IsNullOrWhiteSpace( sampleMonitor.HttpUsername ) );
            Assert.True( string.IsNullOrWhiteSpace( sampleMonitor.HttpPassword ) );
            Assert.NotNull( sampleMonitor.Interval );
            Assert.NotNull( sampleMonitor.Status );
            Assert.Null( sampleMonitor.AllTimeUptimeRatio );
            Assert.Null( sampleMonitor.CustomUptimeRanges );
            Assert.Null( sampleMonitor.AverageResponseTime );
            Assert.Null( sampleMonitor.CustomHttpHeaders );
            Assert.Null( sampleMonitor.CustomHttpStatuses );
            Assert.Null( sampleMonitor.HttpMethod );
            Assert.Null( sampleMonitor.PostType );
            Assert.True( string.IsNullOrWhiteSpace( sampleMonitor.PostValue ) );
            Assert.Null( sampleMonitor.PostContentType );
            Assert.Null( sampleMonitor.CustomUptimeRatio );
            Assert.Null( sampleMonitor.CustomDowntimeDurations );
            Assert.Null( sampleMonitor.Logs );
            Assert.Null( sampleMonitor.ResponseTimes );
            Assert.Null( sampleMonitor.AlertContacts );
            Assert.Null( sampleMonitor.MaintenanceWindows );
            Assert.Null( sampleMonitor.SSL );

            // Timezone
            Assert.Null( result.Timezone );

            // Error
            Assert.Null( result.Error );
        }

        [Fact]
        public async Task GetMonitors_SingleMonitor()
        {
            MonitorsResult allMonitors = await _goodRobot.GetMonitorsAsync();

            Assert.NotNull( allMonitors.Monitors );

            Monitor testMonitor = allMonitors.Monitors.FirstOrDefault();

            MonitorsResult result = await _goodRobot.GetMonitorsAsync( testMonitor.Id.Value );

            // Status
            Assert.Equal( Status.ok, result.Status );

            // Limit
            Assert.NotNull( result.Pagination.Limit );

            // Offset
            Assert.NotNull( result.Pagination.Offset );

            // Total
            Assert.NotNull( result.Pagination.Total );

            // Base Monitor
            Assert.Null( result.BaseMonitor );

            // Monitors
            Monitor sampleMonitor = result.Monitors.FirstOrDefault();

            Assert.NotNull( sampleMonitor.Id );
            Assert.False( string.IsNullOrWhiteSpace( sampleMonitor.FriendlyName ) );
            Assert.False( string.IsNullOrWhiteSpace( sampleMonitor.URL ) );
            Assert.NotNull( sampleMonitor.MonitorType );
            if ( sampleMonitor.MonitorType == MonitorType.Keyword )
            {
                Assert.False( string.IsNullOrWhiteSpace( sampleMonitor.KeywordValue ) );
            }
            Assert.True( string.IsNullOrWhiteSpace( sampleMonitor.HttpUsername ) );
            Assert.True( string.IsNullOrWhiteSpace( sampleMonitor.HttpPassword ) );
            Assert.NotNull( sampleMonitor.Interval );
            Assert.NotNull( sampleMonitor.Status );
            Assert.Null( sampleMonitor.AllTimeUptimeRatio );
            Assert.Null( sampleMonitor.CustomUptimeRanges );
            Assert.Null( sampleMonitor.AverageResponseTime );
            Assert.Null( sampleMonitor.CustomHttpHeaders );
            Assert.Null( sampleMonitor.CustomHttpStatuses );
            Assert.Null( sampleMonitor.HttpMethod );
            Assert.Null( sampleMonitor.PostType );
            Assert.True( string.IsNullOrWhiteSpace( sampleMonitor.PostValue ) );
            Assert.Null( sampleMonitor.PostContentType );
            Assert.Null( sampleMonitor.CustomUptimeRatio );
            Assert.Null( sampleMonitor.CustomDowntimeDurations );
            Assert.Null( sampleMonitor.Logs );
            Assert.Null( sampleMonitor.ResponseTimes );
            Assert.Null( sampleMonitor.AlertContacts );
            Assert.Null( sampleMonitor.MaintenanceWindows );
            Assert.Null( sampleMonitor.SSL );

            // Timezone
            Assert.Null( result.Timezone );

            // Error
            Assert.Null( result.Error );
        }

        [Fact]
        public async Task GetMonitors_WithRequest()
        {
            MonitorsResult allMonitors = await _goodRobot.GetMonitorsAsync();

            Assert.NotNull( allMonitors.Monitors );
            DateTime currentDate = DateTime.UtcNow;

            List<int> monitorIds = allMonitors.Monitors.Select( x => x.Id.Value ).ToList();

            MonitorsRequest request = new MonitorsRequest
            {
                Monitors = monitorIds,
                MonitorTypes = new List<MonitorType> { MonitorType.Keyword, MonitorType.HTTP },
                Statuses = new List<MonitorStatus> { MonitorStatus.Up, MonitorStatus.Down },
                CustomUptimeRatios = new List<int> { 7, 30, 45 },
                CustomUptimeRanges = new List<Tuple<DateTime, DateTime>> { new Tuple<DateTime, DateTime>( currentDate.AddDays( -7 ), currentDate.AddDays( -3 ) ), new Tuple<DateTime, DateTime>( currentDate.AddDays( -10 ), currentDate.AddDays( -8 ) ) },
                AllTimeUptimeRatio = true,
                AllTimeUptimeDurations = true,
                IncludeLogs = true,
                LogsStartDate = currentDate.AddDays( -150 ),
                LogsEndDate = currentDate,
                LogTypes = new List<LogType> { LogType.Down, LogType.Paused, LogType.Up },
                LogsLimit = 5,
                ResponseTimes = true,
                ResponseTimesLimit = 5,
                ResponseTimesAverage = 30,
                // Missing Response Times Start and End Date
                AlertContacts = true,
                MaintenanceWindows = true,
                IncludeSSL = true,
                Timezone = true,
                Offset = 1,
                Limit = 3,
                Search = "Home"
            };

            MonitorsResult result = await _goodRobot.GetMonitorsAsync( request );

            // Status
            Assert.Equal( Status.ok, result.Status );

            // Limit
            Assert.Equal( 3, result.Pagination.Limit );

            // Offset
            Assert.Equal( 1, result.Pagination.Offset );

            // Total
            Assert.NotNull( result.Pagination.Total );

            // Base Monitor
            Assert.Null( result.BaseMonitor );

            // Monitors
            Monitor sampleMonitor = result.Monitors.FirstOrDefault();

            Assert.NotNull( sampleMonitor.Id );
            Assert.False( string.IsNullOrWhiteSpace( sampleMonitor.FriendlyName ) );
            Assert.False( string.IsNullOrWhiteSpace( sampleMonitor.URL ) );
            Assert.NotNull( sampleMonitor.MonitorType );
            if ( sampleMonitor.MonitorType == MonitorType.Keyword )
            {
                Assert.False( string.IsNullOrWhiteSpace( sampleMonitor.KeywordValue ) );
            }
            Assert.True( string.IsNullOrWhiteSpace( sampleMonitor.HttpUsername ) );
            Assert.True( string.IsNullOrWhiteSpace( sampleMonitor.HttpPassword ) );
            Assert.NotNull( sampleMonitor.Interval );
            Assert.NotNull( sampleMonitor.Status );
            Assert.NotNull( sampleMonitor.AllTimeUptimeRatio );
            Assert.NotNull( sampleMonitor.CustomUptimeRanges );
            Assert.NotNull( sampleMonitor.AverageResponseTime );
            Assert.Null( sampleMonitor.CustomHttpHeaders );
            Assert.Null( sampleMonitor.CustomHttpStatuses );
            Assert.Null( sampleMonitor.HttpMethod );
            Assert.Null( sampleMonitor.PostType );
            Assert.True( string.IsNullOrWhiteSpace( sampleMonitor.PostValue ) );
            Assert.Null( sampleMonitor.PostContentType );
            Assert.NotNull( sampleMonitor.CustomUptimeRatio );
            Assert.NotNull( sampleMonitor.CustomDowntimeDurations );

            // Logs
            Assert.NotNull( sampleMonitor.Logs );
            Assert.True( sampleMonitor.Logs.Count <= 5 );
            Assert.NotNull( sampleMonitor.Logs[ 0 ].LogType );
            Assert.NotNull( sampleMonitor.Logs[ 0 ].DateTime );
            Assert.NotNull( sampleMonitor.Logs[ 0 ].Duration );
            Assert.NotNull( sampleMonitor.Logs[ 0 ].Reason.Code );
            Assert.False( string.IsNullOrWhiteSpace( sampleMonitor.Logs[ 0 ].Reason.Detail ) );

            Assert.NotNull( sampleMonitor.ResponseTimes );
            Assert.True( sampleMonitor.ResponseTimes.Count <= 5 );
            Assert.NotNull( sampleMonitor.AlertContacts );
            Assert.NotNull( sampleMonitor.AlertContacts[ 0 ].Threshold );
            Assert.NotNull( sampleMonitor.AlertContacts[ 0 ].Recurrence );
            Assert.NotNull( sampleMonitor.MaintenanceWindows );

            // SSL
            Assert.NotNull( sampleMonitor.SSL );
            Assert.False( string.IsNullOrWhiteSpace( sampleMonitor.SSL.Brand ) );
            Assert.False( string.IsNullOrWhiteSpace( sampleMonitor.SSL.Product ) );
            Assert.NotNull( sampleMonitor.SSL.Expires );
            Assert.NotNull( sampleMonitor.SSL.IgnoreErrors );
            Assert.NotNull( sampleMonitor.SSL.DisableNotifications );

            // Timezone
            Assert.NotNull( result.Timezone );

            // Error
            Assert.Null( result.Error );
        }

        [Fact]
        public async Task GetMonitors_WithRequest_BadLogDates()
        {
            MonitorsResult allMonitors = await _goodRobot.GetMonitorsAsync();

            Assert.NotNull( allMonitors.Monitors );
            DateTime currentDate = DateTime.UtcNow;

            List<int> monitorIds = allMonitors.Monitors.Select( x => x.Id.Value ).ToList();

            MonitorsRequest request = new MonitorsRequest
            {
                IncludeLogs = true,
                LogsStartDate = currentDate.AddDays( -150 )
            };

            MonitorsResult result = await _goodRobot.GetMonitorsAsync( request );

            // Status
            Assert.Equal( Status.fail, result.Status );

            // Pagination
            Assert.Null( result.Pagination );

            // Base Monitor
            Assert.Null( result.BaseMonitor );

            // Monitors
            Assert.Null( result.Monitors );

            // Timezone
            Assert.Null( result.Timezone );

            // Error
            Assert.NotNull( result.Error );
            Assert.Equal( "Internal Exception", result.Error.Explanation );
            Assert.Equal( "Both the Start and End date must be provided for Logs", result.Error.Message );
        }

        #endregion

        #region Alert Contacts

        [Fact]
        public async Task GetAlertContacts()
        {
            AlertContactsResult result = await _goodRobot.GetAlertContactsAsync();

            // Status
            Assert.Equal( Status.ok, result.Status );

            // Limit
            Assert.NotNull( result.Limit );

            // Offset
            Assert.NotNull( result.Offset );

            // Total
            Assert.NotNull( result.Total );

            // Base Alert Contact
            Assert.Null( result.BaseAlertContact );

            // Alert Contacts
            Assert.NotNull( result.AlertContacts );
            Assert.True( result.AlertContacts[ 0 ].Id.HasValue );
            Assert.True( result.AlertContacts[ 0 ].ContactStatus.HasValue );
            Assert.True( result.AlertContacts[ 0 ].ContactType.HasValue );
            Assert.True( !string.IsNullOrWhiteSpace( result.AlertContacts[ 0 ].FriendlyName ) );
            Assert.True( !string.IsNullOrWhiteSpace( result.AlertContacts[ 0 ].Value ) );

            // Error
            Assert.Null( result.Error );

            // Pagination
            Assert.Null( result.Pagination );
        }

        [Fact]
        public async Task GetAlertContacts_SingleContact()
        {
            AlertContactsResult allAlertContacts = await _goodRobot.GetAlertContactsAsync();

            // Make sure something came back for us to work with. 
            Assert.NotNull( allAlertContacts.AlertContacts );

            // Use the last contact created to work arround Issue #26 https://github.com/IanKnighton/SharpenUp/issues/26
            AlertContact testContact = allAlertContacts.AlertContacts.LastOrDefault();

            AlertContactsResult result = await _goodRobot.GetAlertContactsAsync( testContact.Id.Value );

            // Status
            Assert.Equal( Status.ok, result.Status );

            // Limit
            Assert.NotNull( result.Limit );

            // Offset
            Assert.NotNull( result.Offset );

            // Total
            Assert.NotNull( result.Total );

            // Base Alert Contact
            Assert.Null( result.BaseAlertContact );

            // Alert Contacts
            Assert.NotNull( result.AlertContacts );
            Assert.True( result.AlertContacts[ 0 ].Id.HasValue );
            Assert.True( result.AlertContacts[ 0 ].ContactStatus.HasValue );
            Assert.True( result.AlertContacts[ 0 ].ContactType.HasValue );
            Assert.True( !string.IsNullOrWhiteSpace( result.AlertContacts[ 0 ].FriendlyName ) );
            Assert.True( !string.IsNullOrWhiteSpace( result.AlertContacts[ 0 ].Value ) );

            // Error
            Assert.Null( result.Error );

            // Pagination
            Assert.Null( result.Pagination );
        }

        [Fact]
        public async Task GetAlertContacts_WithRequest()
        {
            AlertContactsResult allAlertContacts = await _goodRobot.GetAlertContactsAsync();

            // Make sure something came back for us to work with. 
            Assert.NotNull( allAlertContacts.AlertContacts );

            List<int> contactIds = allAlertContacts.AlertContacts.Select( x => x.Id.Value ).ToList();

            // Make sure we have ID's to work with.
            Assert.NotNull( contactIds );

            AlertContactsRequest request = new AlertContactsRequest
            {
                AlertContacts = new List<int> { contactIds[ 1 ], contactIds[ 2 ], contactIds[ 3 ] },
                Offset = 2,
                Limit = 20
            };

            AlertContactsResult result = await _goodRobot.GetAlertContactsAsync( request );

            // Status
            Assert.Equal( Status.ok, result.Status );

            // Limit
            Assert.Equal( 20, result.Limit.Value );

            // Offset
            Assert.Equal( 2, result.Offset.Value );

            // Total
            Assert.True( result.Total.HasValue );

            // Base Alert Contact
            Assert.Null( result.BaseAlertContact );

            // Alert Contacts
            Assert.NotNull( result.AlertContacts );
            Assert.True( result.AlertContacts[ 0 ].Id.HasValue );
            Assert.True( result.AlertContacts[ 0 ].ContactStatus.HasValue );
            Assert.True( result.AlertContacts[ 0 ].ContactType.HasValue );
            Assert.True( !string.IsNullOrWhiteSpace( result.AlertContacts[ 0 ].FriendlyName ) );
            Assert.True( !string.IsNullOrWhiteSpace( result.AlertContacts[ 0 ].Value ) );

            // Error
            Assert.Null( result.Error );

            // Pagination
            Assert.Null( result.Pagination );
        }

        [Fact]
        public async Task CreateAlertContact_BadParameters()
        {
            AlertContactsResult result = await _goodRobot.CreateAlertContactAsync( ContactType.Email, "", "" );

            // Status
            Assert.Equal( Status.fail, result.Status );

            // Limit
            Assert.Null( result.Limit );

            // Offset
            Assert.Null( result.Offset );

            // Total
            Assert.Null( result.Total );

            // Base Alert Contact
            Assert.Null( result.BaseAlertContact );

            // Alert Contacts
            Assert.Null( result.AlertContacts );

            // Error
            Assert.NotNull( result.Error );
            Assert.Equal( "Inner Exception", result.Error.Explanation );
            Assert.Equal( "Incorrent Parameters", result.Error.Message );

            // Pagination
            Assert.Null( result.Pagination );
        }

        [Fact]
        public async Task UpdateAlertContact_BadId()
        {
            AlertContactsResult result = await _goodRobot.UpdateAlertContactAsync( 12345, "", "" );

            // Status
            Assert.Equal( Status.fail, result.Status );

            // Limit
            Assert.Null( result.Limit );

            // Offset
            Assert.Null( result.Offset );

            // Total
            Assert.Null( result.Total );

            // Base Alert Contact
            Assert.Null( result.BaseAlertContact );

            // Alert Contacts
            Assert.Null( result.AlertContacts );

            // Error
            Assert.NotNull( result.Error );
            Assert.Equal( "Inner Exception", result.Error.Explanation );
            Assert.Equal( "Contact does not exist.", result.Error.Message );

            // Pagination
            Assert.Null( result.Pagination );
        }

        [Fact]
        public async Task DeleteAlertContact_BadId()
        {
            AlertContactsResult result = await _goodRobot.DeleteAlertContactsAsync( 12345 );

            // Status
            Assert.Equal( Status.fail, result.Status );

            // Limit
            Assert.Null( result.Limit );

            // Offset
            Assert.Null( result.Offset );

            // Total
            Assert.Null( result.Total );

            // Base Alert Contact
            Assert.Null( result.BaseAlertContact );

            // Alert Contacts
            Assert.Null( result.AlertContacts );

            // Error
            Assert.NotNull( result.Error );
            Assert.Equal( "Inner Exception", result.Error.Explanation );
            Assert.Equal( "No Alert Contact Found", result.Error.Message );

            // Pagination
            Assert.Null( result.Pagination );
        }

        [Fact]
        public async Task AlertContacts_CRUDOperations()
        {
            // Create the Contact
            AlertContactsResult alertContact = await _goodRobot.CreateAlertContactAsync( ContactType.Email, "fake@fakest.org", "Fake Fakerson" );

            // Status
            Assert.Equal( Status.ok, alertContact.Status );

            // Limit
            Assert.Null( alertContact.Limit );

            // Offset
            Assert.Null( alertContact.Offset );

            // Total
            Assert.Null( alertContact.Total );

            // Base Alert Contact
            Assert.NotNull( alertContact.BaseAlertContact );

            // Alert Contacts
            Assert.Null( alertContact.AlertContacts );

            // Validate the contact was fully created.
            AlertContactsResult result = await _goodRobot.GetAlertContactsAsync( alertContact.BaseAlertContact.Id.Value );

            // Status
            Assert.Equal( Status.ok, result.Status );

            // Limit
            Assert.NotNull( result.Limit );

            // Offset
            Assert.NotNull( result.Offset );

            // Total
            Assert.NotNull( result.Total );

            // Base Alert Contact
            Assert.Null( result.BaseAlertContact );

            // Alert Contacts
            Assert.NotNull( result.AlertContacts );
            Assert.Equal( alertContact.BaseAlertContact.Id.Value, result.AlertContacts[ 0 ].Id.Value );
            Assert.Equal( ContactStatus.NotActivated, result.AlertContacts[ 0 ].ContactStatus );
            Assert.Equal( ContactType.Email, result.AlertContacts[ 0 ].ContactType );
            Assert.Equal( "Fake Fakerson", result.AlertContacts[ 0 ].FriendlyName );
            Assert.Equal( "fake@fakest.org", result.AlertContacts[ 0 ].Value );

            // Error
            Assert.Null( result.Error );

            // Pagination
            Assert.Null( result.Pagination );

            // Update the Contact
            AlertContactsResult updatedAlertContact = await _goodRobot.UpdateAlertContactAsync( alertContact.BaseAlertContact.Id.Value, "Really Faking", "" );

            // Status
            Assert.Equal( Status.ok, updatedAlertContact.Status );

            // Limit
            Assert.Null( updatedAlertContact.Limit );

            // Offset
            Assert.Null( updatedAlertContact.Offset );

            // Total
            Assert.Null( updatedAlertContact.Total );

            // Base Alert Contact
            Assert.Null( updatedAlertContact.BaseAlertContact );

            // Alert Contacts
            Assert.Null( updatedAlertContact.AlertContacts );

            // Pull back the whole result. 
            AlertContactsResult updatedResult = await _goodRobot.GetAlertContactsAsync( alertContact.BaseAlertContact.Id.Value );

            // Status
            Assert.Equal( Status.ok, updatedResult.Status );

            // Limit
            Assert.NotNull( updatedResult.Limit );

            // Offset
            Assert.NotNull( updatedResult.Offset );

            // Total
            Assert.NotNull( updatedResult.Total );

            // Base Alert Contact
            Assert.Null( updatedResult.BaseAlertContact );

            // Alert Contacts
            Assert.NotNull( updatedResult.AlertContacts );
            Assert.Equal( alertContact.BaseAlertContact.Id.Value, updatedResult.AlertContacts[ 0 ].Id.Value );
            Assert.Equal( ContactStatus.NotActivated, updatedResult.AlertContacts[ 0 ].ContactStatus );
            Assert.Equal( ContactType.Email, updatedResult.AlertContacts[ 0 ].ContactType );
            Assert.Equal( "Really Faking", updatedResult.AlertContacts[ 0 ].FriendlyName );
            Assert.Equal( "fake@fakest.org", updatedResult.AlertContacts[ 0 ].Value );

            // Error
            Assert.Null( result.Error );

            // Pagination
            Assert.Null( result.Pagination );

            // Delete the Contact
            AlertContactsResult deletedAlertContact = await _goodRobot.DeleteAlertContactsAsync( alertContact.BaseAlertContact.Id.Value );

            // Status
            Assert.Equal( Status.ok, deletedAlertContact.Status );

            // Limit
            Assert.Null( deletedAlertContact.Limit );

            // Offset
            Assert.Null( deletedAlertContact.Offset );

            // Total
            Assert.Null( deletedAlertContact.Total );

            // Base Alert Contact
            Assert.Null( deletedAlertContact.BaseAlertContact );

            // Alert Contacts
            Assert.Null( deletedAlertContact.AlertContacts );
        }

        #endregion

        #region Maintenance Windows

        [Fact]
        public async Task GetMaintenanceWindows()
        {
            MaintenanceWindowsResult result = await _goodRobot.GetMaintenanceWindowsAsync();

            // Status
            Assert.Equal( Status.ok, result.Status );

            // Limit
            Assert.NotNull( result.Pagination.Limit );

            // Offset
            Assert.NotNull( result.Pagination.Offset );

            // Total
            Assert.NotNull( result.Pagination.Total );

            // Base Maintenance Window
            Assert.Null( result.BaseMaintenanceWindow );

            // Maintenance Windows
            Assert.NotNull( result.MaintenanceWindows );
            Assert.NotNull( result.MaintenanceWindows[ 0 ].Id );
            Assert.NotNull( result.MaintenanceWindows[ 0 ].MaintenanceWindowType );
            Assert.True( !string.IsNullOrWhiteSpace( result.MaintenanceWindows[ 0 ].FriendlyName ) );
            if ( result.MaintenanceWindows[ 0 ].MaintenanceWindowType == MaintenanceWindowType.Monthly || result.MaintenanceWindows[ 0 ].MaintenanceWindowType == MaintenanceWindowType.Weekly )
            {
                Assert.True( !string.IsNullOrWhiteSpace( result.MaintenanceWindows[ 0 ].Value ) );
            }
            Assert.NotNull( result.MaintenanceWindows[ 0 ].StartTime );
            Assert.NotNull( result.MaintenanceWindows[ 0 ].Duration );
            Assert.NotNull( result.MaintenanceWindows[ 0 ].MaintenanceWindowStatus );
        }

        [Fact]
        public async Task GetMaintenanceWindows_SingleWindow()
        {
            MaintenanceWindowsResult allMaintenanceWindows = await _goodRobot.GetMaintenanceWindowsAsync();

            Assert.NotNull( allMaintenanceWindows.MaintenanceWindows );

            MaintenanceWindow testWindow = allMaintenanceWindows.MaintenanceWindows.FirstOrDefault();

            MaintenanceWindowsResult result = await _goodRobot.GetMaintenanceWindowsAsync( testWindow.Id.Value );

            // Status
            Assert.Equal( Status.ok, result.Status );

            // Limit
            Assert.NotNull( result.Pagination.Limit );

            // Offset
            Assert.NotNull( result.Pagination.Offset );

            // Total
            Assert.NotNull( result.Pagination.Total );

            // Base Maintenance Window
            Assert.Null( result.BaseMaintenanceWindow );

            // Maintenance Windows
            Assert.NotNull( result.MaintenanceWindows );
            Assert.NotNull( result.MaintenanceWindows[ 0 ].Id );
            Assert.NotNull( result.MaintenanceWindows[ 0 ].MaintenanceWindowType );
            Assert.True( !string.IsNullOrWhiteSpace( result.MaintenanceWindows[ 0 ].FriendlyName ) );
            if ( result.MaintenanceWindows[ 0 ].MaintenanceWindowType == MaintenanceWindowType.Monthly || result.MaintenanceWindows[ 0 ].MaintenanceWindowType == MaintenanceWindowType.Weekly )
            {
                Assert.True( !string.IsNullOrWhiteSpace( result.MaintenanceWindows[ 0 ].Value ) );
            }
            Assert.NotNull( result.MaintenanceWindows[ 0 ].StartTime );
            Assert.NotNull( result.MaintenanceWindows[ 0 ].Duration );
            Assert.NotNull( result.MaintenanceWindows[ 0 ].MaintenanceWindowStatus );
        }

        [Fact]
        public async Task GetMaintenanceWindows_WithRequest()
        {
            MaintenanceWindowsResult allMaintenanceWindows = await _goodRobot.GetMaintenanceWindowsAsync();

            Assert.NotNull( allMaintenanceWindows.MaintenanceWindows );

            List<int> maintenanceWindowIds = allMaintenanceWindows.MaintenanceWindows.Select( x => x.Id.Value ).ToList();

            Assert.NotNull( maintenanceWindowIds );

            MaintenanceWindowsRequest request = new MaintenanceWindowsRequest
            {
                MaintenanceWindows = maintenanceWindowIds,
                Offset = 1,
                Limit = 20
            };

            MaintenanceWindowsResult result = await _goodRobot.GetMaintenanceWindowsAsync( request );


            // Status
            Assert.Equal( Status.ok, result.Status );

            // Limit
            Assert.Equal( 20, result.Pagination.Limit.Value );

            // Offset
            Assert.Equal( 1, result.Pagination.Offset.Value );

            // Total
            Assert.NotNull( result.Pagination.Total );

            // Base Maintenance Window
            Assert.Null( result.BaseMaintenanceWindow );

            // Maintenance Windows
            Assert.NotNull( result.MaintenanceWindows );
            Assert.NotNull( result.MaintenanceWindows[ 0 ].Id );
            Assert.NotNull( result.MaintenanceWindows[ 0 ].MaintenanceWindowType );
            Assert.True( !string.IsNullOrWhiteSpace( result.MaintenanceWindows[ 0 ].FriendlyName ) );
            if ( result.MaintenanceWindows[ 0 ].MaintenanceWindowType == MaintenanceWindowType.Monthly || result.MaintenanceWindows[ 0 ].MaintenanceWindowType == MaintenanceWindowType.Weekly )
            {
                Assert.True( !string.IsNullOrWhiteSpace( result.MaintenanceWindows[ 0 ].Value ) );
            }
            Assert.NotNull( result.MaintenanceWindows[ 0 ].StartTime );
            Assert.NotNull( result.MaintenanceWindows[ 0 ].Duration );
            Assert.NotNull( result.MaintenanceWindows[ 0 ].MaintenanceWindowStatus );
        }

        [Fact]
        public async Task CreateMaintenanceWindow_BadParameters_FriendlyName()
        {
            MaintenanceWindowsResult result = await _goodRobot.CreateMaintenanceWindowAsync( "", MaintenanceWindowType.Daily, "", new TimeSpan( 20, 0, 0 ), 60 );

            // Status
            Assert.Equal( Status.fail, result.Status );

            // Pagination
            Assert.Null( result.Pagination );

            // Base Maintenance Window
            Assert.Null( result.BaseMaintenanceWindow );

            // Maintenance Windows
            Assert.Null( result.MaintenanceWindows );

            // Error
            Assert.NotNull( result.Error );
            Assert.Equal( _defaultExplanation, result.Error.Explanation );
            Assert.Equal( "No Friendly Name Was Provided", result.Error.Message );
            Assert.Equal( ErrorType.NoFriendlyName, result.Error.ErrorType );
        }

        [Fact]
        public async Task CreateMaintenanceWindow_BadParameters_Value()
        {
            MaintenanceWindowsResult result = await _goodRobot.CreateMaintenanceWindowAsync( "Fake Name", MaintenanceWindowType.Weekly, "", new TimeSpan( 20, 0, 0 ), 60 );

            // Status
            Assert.Equal( Status.fail, result.Status );

            // Pagination
            Assert.Null( result.Pagination );

            // Base Maintenance Window
            Assert.Null( result.BaseMaintenanceWindow );

            // Maintenance Windows
            Assert.Null( result.MaintenanceWindows );

            // Error
            Assert.NotNull( result.Error );
            Assert.Equal( _defaultExplanation, result.Error.Explanation );
            Assert.Equal( "A Value must be provided when using the Weekly or Monthly monitor type.", result.Error.Message );
            Assert.Equal( ErrorType.MaintenanceWindow_WindowTypeRequired, result.Error.ErrorType );
        }

        [Fact]
        public async Task UpdateMaintenanceWindow_BadId()
        {
            MaintenanceWindowsResult result = await _goodRobot.UpdateMaintenanceWindowAsync( 45, "", "", new TimeSpan( 2, 2, 2 ), 60 );

            // Status
            Assert.Equal( Status.fail, result.Status );

            // Pagination
            Assert.Null( result.Pagination );

            // Base Maintenance Window
            Assert.Null( result.BaseMaintenanceWindow );

            // Maintenance Windows
            Assert.Null( result.MaintenanceWindows );

            // Error
            Assert.NotNull( result.Error );
            Assert.Equal( "not_found", result.Error.Explanation );
            Assert.Equal( "Maintenance Window not found.", result.Error.Message );
        }

        [Fact]
        public async Task DeleteMaintenanceWindow_BadId()
        {
            MaintenanceWindowsResult result = await _goodRobot.DeleteMaintenanceWindowAsync( 1234 );

            // Status
            Assert.Equal( Status.fail, result.Status );

            // Pagination
            Assert.Null( result.Pagination );

            // Base Maintenance Window
            Assert.Null( result.BaseMaintenanceWindow );

            // Maintenance Windows
            Assert.Null( result.MaintenanceWindows );

            // Error
            Assert.NotNull( result.Error );
            Assert.Equal( "not_found", result.Error.Explanation );
            Assert.Equal( "Maintenance Window not found.", result.Error.Message );
        }

        [Fact]
        public async Task MaintenanceWindows_CRUDOperations()
        {
            MaintenanceWindowsResult maintenanceWindow = await _goodRobot.CreateMaintenanceWindowAsync( "Fake Window", MaintenanceWindowType.Monthly, "1-2-5", new TimeSpan( 2, 30, 00 ), 30 );

            // Status
            Assert.Equal( Status.ok, maintenanceWindow.Status );

            // Pagination
            Assert.Null( maintenanceWindow.Pagination );

            // Base Maintenance Window
            Assert.NotNull( maintenanceWindow.BaseMaintenanceWindow );

            // Maintenance Windows
            Assert.Null( maintenanceWindow.MaintenanceWindows );

            // Error
            Assert.Null( maintenanceWindow.Error );

            // Validate the Maintenance Window was Created
            MaintenanceWindowsResult result = await _goodRobot.GetMaintenanceWindowsAsync( maintenanceWindow.BaseMaintenanceWindow.Id.Value );

            // Status
            Assert.Equal( Status.ok, result.Status );

            // Limit
            Assert.NotNull( result.Pagination.Limit );

            // Offset
            Assert.NotNull( result.Pagination.Offset );

            // Total
            Assert.NotNull( result.Pagination.Total );

            // Base Maintenance Window
            Assert.Null( result.BaseMaintenanceWindow );

            // Maintenance Windows
            Assert.NotNull( result.MaintenanceWindows );
            Assert.NotNull( result.MaintenanceWindows.Last().Id );
            Assert.NotNull( result.MaintenanceWindows.Last().MaintenanceWindowType );
            Assert.Equal( "Fake Window", result.MaintenanceWindows.Last().FriendlyName );
            if ( result.MaintenanceWindows.Last().MaintenanceWindowType == MaintenanceWindowType.Monthly || result.MaintenanceWindows.Last().MaintenanceWindowType == MaintenanceWindowType.Weekly )
            {
                Assert.Equal( "1,2,5", result.MaintenanceWindows.Last().Value );
            }
            Assert.Equal( new TimeSpan( 8, 30, 00 ), result.MaintenanceWindows.Last().StartTime );
            Assert.Equal( 30, result.MaintenanceWindows.Last().Duration );
            Assert.NotNull( result.MaintenanceWindows.Last().MaintenanceWindowStatus );

            // Update the Maintenance Window
            MaintenanceWindowsResult updatedMaintenanceWindow = await _goodRobot.UpdateMaintenanceWindowAsync( maintenanceWindow.BaseMaintenanceWindow.Id.Value, "Real Fake", "2", new TimeSpan( 2, 32, 00 ), 55 );

            // Status
            Assert.Equal( Status.ok, updatedMaintenanceWindow.Status );

            // Pagination
            Assert.Null( updatedMaintenanceWindow.Pagination );

            // Base Maintenance Window
            Assert.NotNull( updatedMaintenanceWindow.BaseMaintenanceWindow );

            // Maintenance Windows
            Assert.Null( updatedMaintenanceWindow.MaintenanceWindows );

            // Error
            Assert.Null( updatedMaintenanceWindow.Error );

            // Pull back the full Window
            MaintenanceWindowsResult updatedResult = await _goodRobot.GetMaintenanceWindowsAsync( maintenanceWindow.BaseMaintenanceWindow.Id.Value );

            // Status
            Assert.Equal( Status.ok, updatedResult.Status );

            // Limit
            Assert.NotNull( updatedResult.Pagination.Limit );

            // Offset
            Assert.NotNull( updatedResult.Pagination.Offset );

            // Total
            Assert.NotNull( updatedResult.Pagination.Total );

            // Base Maintenance Window
            Assert.Null( updatedResult.BaseMaintenanceWindow );

            // Maintenance Windows
            Assert.NotNull( updatedResult.MaintenanceWindows );
            Assert.NotNull( updatedResult.MaintenanceWindows.Last().Id );
            Assert.NotNull( updatedResult.MaintenanceWindows.Last().MaintenanceWindowType );
            Assert.Equal( "Real Fake", updatedResult.MaintenanceWindows.Last().FriendlyName );
            if ( updatedResult.MaintenanceWindows.Last().MaintenanceWindowType == MaintenanceWindowType.Monthly || updatedResult.MaintenanceWindows.Last().MaintenanceWindowType == MaintenanceWindowType.Weekly )
            {
                Assert.Equal( "2", updatedResult.MaintenanceWindows.Last().Value );
            }
            Assert.Equal( new TimeSpan( 2, 32, 00 ), updatedResult.MaintenanceWindows.Last().StartTime );
            Assert.Equal( 55, updatedResult.MaintenanceWindows.Last().Duration );
            Assert.NotNull( updatedResult.MaintenanceWindows.Last().MaintenanceWindowStatus );

            // Delete the Window
            MaintenanceWindowsResult deletedMaintenanceWindow = await _goodRobot.DeleteMaintenanceWindowAsync( maintenanceWindow.BaseMaintenanceWindow.Id.Value );

            // Status
            Assert.Equal( Status.ok, deletedMaintenanceWindow.Status );

            // Pagination
            Assert.Null( deletedMaintenanceWindow.Pagination );

            // Base Maintenance Window
            Assert.Null( deletedMaintenanceWindow.BaseMaintenanceWindow );

            // Maintenance Windows
            Assert.Null( deletedMaintenanceWindow.MaintenanceWindows );

            // Error
            Assert.Null( deletedMaintenanceWindow.Error );
        }

        #endregion

        #region Public Status Pages

        [Fact]
        public async Task GetPublicStatusPages()
        {
            PublicStatusPageResult result = await _goodRobot.GetPublicStatusPagesAsync();

            // Status
            Assert.Equal( Status.ok, result.Status );

            // Limit
            Assert.NotNull( result.Pagination.Limit );

            // Offset
            Assert.NotNull( result.Pagination.Offset );

            // Total
            Assert.NotNull( result.Pagination.Total );

            // Base Public Status Page
            Assert.Null( result.BasePublicStatusPage );

            // Public Status Pages
            Assert.NotNull( result.PublicStatusPages );
            Assert.NotNull( result.PublicStatusPages[ 0 ].Id );
            Assert.True( !string.IsNullOrWhiteSpace( result.PublicStatusPages[ 0 ].FriendlyName ) );
            Assert.NotNull( result.PublicStatusPages[ 0 ].Monitors );
            Assert.True( !string.IsNullOrWhiteSpace( result.PublicStatusPages[ 0 ].StandardDomain ) );
            Assert.True( string.IsNullOrEmpty( result.PublicStatusPages[ 0 ].CustomDomain ) );
            Assert.True( string.IsNullOrEmpty( result.PublicStatusPages[ 0 ].Password ) );
            Assert.NotNull( result.PublicStatusPages[ 0 ].PublicStatusPageSort );
            Assert.NotNull( result.PublicStatusPages[ 0 ].PublicStatusPageStatus );

            // Error
            Assert.Null( result.Error );
        }

        [Fact]
        public async Task GetPublicStatusPages_SinglePage()
        {
            PublicStatusPageResult allStatusPages = await _goodRobot.GetPublicStatusPagesAsync();

            Assert.NotNull( allStatusPages.PublicStatusPages );

            PublicStatusPageResult result = await _goodRobot.GetPublicStatusPagesAsync( allStatusPages.PublicStatusPages[ 0 ].Id.Value );

            // Status
            Assert.Equal( Status.ok, result.Status );

            // Limit
            Assert.NotNull( result.Pagination.Limit );

            // Offset
            Assert.NotNull( result.Pagination.Offset );

            // Total
            Assert.NotNull( result.Pagination.Total );

            // Base Public Status Page
            Assert.Null( result.BasePublicStatusPage );

            // Public Status Pages
            Assert.NotNull( result.PublicStatusPages );
            Assert.NotNull( result.PublicStatusPages[ 0 ].Id );
            Assert.True( !string.IsNullOrWhiteSpace( result.PublicStatusPages[ 0 ].FriendlyName ) );
            Assert.NotNull( result.PublicStatusPages[ 0 ].Monitors );
            Assert.True( !string.IsNullOrWhiteSpace( result.PublicStatusPages[ 0 ].StandardDomain ) );
            Assert.True( string.IsNullOrEmpty( result.PublicStatusPages[ 0 ].CustomDomain ) );
            Assert.True( string.IsNullOrEmpty( result.PublicStatusPages[ 0 ].Password ) );
            Assert.NotNull( result.PublicStatusPages[ 0 ].PublicStatusPageSort );
            Assert.NotNull( result.PublicStatusPages[ 0 ].PublicStatusPageStatus );

            // Error
            Assert.Null( result.Error );
        }

        [Fact]
        public async Task GetPublicStatusPages_WithRequest()
        {
            PublicStatusPageResult allStatusPages = await _goodRobot.GetPublicStatusPagesAsync();

            Assert.NotNull( allStatusPages.PublicStatusPages );

            List<int> publicStatusPages = allStatusPages.PublicStatusPages.Select( x => x.Id.Value ).ToList();

            PublicStatusPageRequest request = new PublicStatusPageRequest
            {
                PublicStatusPages = publicStatusPages,
                Offset = 1,
                Limit = 20
            };

            PublicStatusPageResult result = await _goodRobot.GetPublicStatusPagesAsync( request );

            // Status
            Assert.Equal( Status.ok, result.Status );

            // Limit
            Assert.Equal( 20, result.Pagination.Limit );

            // Offset
            Assert.Equal( 1, result.Pagination.Offset );

            // Total
            Assert.NotNull( result.Pagination.Total );

            // Base Public Status Page
            Assert.Null( result.BasePublicStatusPage );

            // Public Status Pages
            Assert.NotNull( result.PublicStatusPages );
            Assert.NotNull( result.PublicStatusPages[ 0 ].Id );
            Assert.True( !string.IsNullOrWhiteSpace( result.PublicStatusPages[ 0 ].FriendlyName ) );
            Assert.NotNull( result.PublicStatusPages[ 0 ].Monitors );
            Assert.True( !string.IsNullOrWhiteSpace( result.PublicStatusPages[ 0 ].StandardDomain ) );
            Assert.True( string.IsNullOrEmpty( result.PublicStatusPages[ 0 ].CustomDomain ) );
            Assert.True( string.IsNullOrEmpty( result.PublicStatusPages[ 0 ].Password ) );
            Assert.NotNull( result.PublicStatusPages[ 0 ].PublicStatusPageSort );
            Assert.NotNull( result.PublicStatusPages[ 0 ].PublicStatusPageStatus );

            // Error
            Assert.Null( result.Error );
        }

        [Fact]
        public async Task CreatePublicStatusPage_BadRequest_FriendlyName()
        {
            PublicStatusPageResult result = await _goodRobot.CreatePublicStatusPageAsync( "", new List<int>() );

            // Status
            Assert.Equal( Status.fail, result.Status );

            // Pagination
            Assert.Null( result.Pagination );

            // Base Public Status Page
            Assert.Null( result.BasePublicStatusPage );

            // Public Status Pages
            Assert.Null( result.PublicStatusPages );

            // Error
            Assert.NotNull( result.Error );
            Assert.Equal( _defaultExplanation, result.Error.Explanation );
            Assert.Equal( "No Friendly Name Was Provided", result.Error.Message );
            Assert.Equal( ErrorType.NoFriendlyName, result.Error.ErrorType ); ;
        }

        [Fact]
        public async Task UpdatePublicStatusPage_BadId()
        {
            PublicStatusPageResult result = await _goodRobot.UpdatePublicStatusPageAsync( 1234, "", null, "", "", PublicStatusPageSort.FriendlyNameAscending );

            // Status
            Assert.Equal( Status.fail, result.Status );

            // Pagination
            Assert.Null( result.Pagination );

            // Base Public Status Page
            Assert.Null( result.BasePublicStatusPage );

            // Public Status Pages
            Assert.Null( result.PublicStatusPages );

            // Error
            Assert.NotNull( result.Error );
            Assert.Equal( _defaultExplanation, result.Error.Explanation );
            Assert.Equal( "No Public Status Page Was Found", result.Error.Message );
            Assert.Equal( ErrorType.PublicStatusPage_NoPageFound, result.Error.ErrorType );
        }

        [Fact]
        public async Task DeletePublicStatusPage_BadId()
        {
            PublicStatusPageResult result = await _goodRobot.DeletePublicStatusPageAsync( 1234 );

            // Status
            Assert.Equal( Status.fail, result.Status );

            // Pagination
            Assert.Null( result.Pagination );

            // Base Public Status Page
            Assert.Null( result.BasePublicStatusPage );

            // Public Status Pages
            Assert.Null( result.PublicStatusPages );

            // Error
            Assert.NotNull( result.Error );
            Assert.Equal( _defaultExplanation, result.Error.Explanation );
            Assert.Equal( "No Public Status Page Was Found", result.Error.Message );
            Assert.Equal( ErrorType.PublicStatusPage_NoPageFound, result.Error.ErrorType );
        }

        [Fact]
        public async Task PublicStatusPages_CRUDOperations()
        {
            PublicStatusPageResult publicStatusPage = await _goodRobot.CreatePublicStatusPageAsync( "Fake Page", null, "fekr.org", "pas$w0Rd", PublicStatusPageSort.FriendlyNameAscending );

            // Status
            Assert.Equal( Status.ok, publicStatusPage.Status );

            // Pagination
            Assert.Null( publicStatusPage.Pagination );

            // Base Public Status Page
            Assert.NotNull( publicStatusPage.BasePublicStatusPage );

            // Public Status Pages
            Assert.Null( publicStatusPage.PublicStatusPages );

            // Error
            Assert.Null( publicStatusPage.Error );

            // Pull back full result
            PublicStatusPageResult result = await _goodRobot.GetPublicStatusPagesAsync( publicStatusPage.BasePublicStatusPage.Id.Value );

            // Status
            Assert.Equal( Status.ok, result.Status );

            // Limit
            Assert.NotNull( result.Pagination.Limit );

            // Offset
            Assert.NotNull( result.Pagination.Offset );

            // Total
            Assert.NotNull( result.Pagination.Total );

            // Base Public Status Page
            Assert.Null( result.BasePublicStatusPage );

            // Public Status Pages
            Assert.NotNull( result.PublicStatusPages );
            Assert.NotNull( result.PublicStatusPages[ 0 ].Id );
            Assert.Equal( "Fake Page", result.PublicStatusPages[ 0 ].FriendlyName );
            Assert.NotNull( result.PublicStatusPages[ 0 ].Monitors );
            Assert.True( !string.IsNullOrWhiteSpace( result.PublicStatusPages[ 0 ].StandardDomain ) );
            Assert.Equal( "https://fekr.org", result.PublicStatusPages[ 0 ].CustomDomain );
            Assert.Null( result.PublicStatusPages[ 0 ].Password );
            Assert.Equal( PublicStatusPageSort.FriendlyNameAscending, result.PublicStatusPages[ 0 ].PublicStatusPageSort );
            Assert.NotNull( result.PublicStatusPages[ 0 ].PublicStatusPageStatus );

            // Error
            Assert.Null( result.Error );

            // Update the Public Status Page
            PublicStatusPageResult updatedPublicStatusPage = await _goodRobot.UpdatePublicStatusPageAsync( publicStatusPage.BasePublicStatusPage.Id.Value, "Still Fake", null, "rejc.net", "", PublicStatusPageSort.FriendlyNameDescending );

            // Status
            Assert.Equal( Status.ok, updatedPublicStatusPage.Status );

            // Pagination
            Assert.Null( updatedPublicStatusPage.Pagination );

            // Base Public Status Page
            Assert.NotNull( updatedPublicStatusPage.BasePublicStatusPage );

            // Public Status Pages
            Assert.Null( updatedPublicStatusPage.PublicStatusPages );

            // Error
            Assert.Null( updatedPublicStatusPage.Error );

            // Pull back the updated result.
            PublicStatusPageResult updatedResult = await _goodRobot.GetPublicStatusPagesAsync( publicStatusPage.BasePublicStatusPage.Id.Value );

            // Status
            Assert.Equal( Status.ok, updatedResult.Status );

            // Limit
            Assert.NotNull( updatedResult.Pagination.Limit );

            // Offset
            Assert.NotNull( updatedResult.Pagination.Offset );

            // Total
            Assert.NotNull( updatedResult.Pagination.Total );

            // Base Public Status Page
            Assert.Null( updatedResult.BasePublicStatusPage );

            // Public Status Pages
            Assert.NotNull( updatedResult.PublicStatusPages );
            Assert.NotNull( updatedResult.PublicStatusPages[ 0 ].Id );
            Assert.Equal( "Still Fake", updatedResult.PublicStatusPages[ 0 ].FriendlyName );
            Assert.NotNull( updatedResult.PublicStatusPages[ 0 ].Monitors );
            Assert.True( !string.IsNullOrWhiteSpace( updatedResult.PublicStatusPages[ 0 ].StandardDomain ) );
            Assert.Equal( "https://rejc.net", updatedResult.PublicStatusPages[ 0 ].CustomDomain );
            Assert.Null( updatedResult.PublicStatusPages[ 0 ].Password );
            Assert.Equal( PublicStatusPageSort.FriendlyNameDescending, updatedResult.PublicStatusPages[ 0 ].PublicStatusPageSort );
            Assert.NotNull( updatedResult.PublicStatusPages[ 0 ].PublicStatusPageStatus );

            // Delete the Public Status Page
            PublicStatusPageResult deletedPublicStatusPage = await _goodRobot.DeletePublicStatusPageAsync( publicStatusPage.BasePublicStatusPage.Id.Value );

            // Status
            Assert.Equal( Status.ok, deletedPublicStatusPage.Status );

            // Pagination
            Assert.Null( deletedPublicStatusPage.Pagination );

            // Base Public Status Page
            Assert.Equal( publicStatusPage.BasePublicStatusPage.Id.Value, deletedPublicStatusPage.BasePublicStatusPage.Id.Value );

            // Public Status Pages
            Assert.Null( deletedPublicStatusPage.PublicStatusPages );

            // Error
            Assert.Null( deletedPublicStatusPage.Error );
        }

        #endregion
    }
}
