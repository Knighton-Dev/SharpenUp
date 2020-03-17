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
            Assert.Equal( "invalid_parameter", result.Error.Type );
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
            Assert.Equal( "Inner Exception", result.Error.Type );
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
            Assert.Equal( "Inner Exception", result.Error.Type );
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
            Assert.Equal( "Inner Exception", result.Error.Type );
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
            Assert.Equal( "Inner Exception", result.Error.Type );
            Assert.Equal( "A Friendly Name is Required", result.Error.Message );
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
            Assert.Equal( "Inner Exception", result.Error.Type );
            Assert.Equal( "A value is required when the Window Type is Weekly or Monthly.", result.Error.Message );
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
            Assert.Equal( "not_found", result.Error.Type );
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
            Assert.Equal( "not_found", result.Error.Type );
            Assert.Equal( "Maintenance Window not found.", result.Error.Message );
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
            Assert.Equal( "Inner Exception", result.Error.Type );
            Assert.Equal( "A Friendly Name is Required", result.Error.Message );
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
            Assert.Equal( "Inner Exception", result.Error.Type );
            Assert.Equal( "No Public Status Page was found!", result.Error.Message );
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
            Assert.Equal( "Inner Exception", result.Error.Type );
            Assert.Equal( "No Public Status Page was found!", result.Error.Message );
        }

        #endregion
    }
}
