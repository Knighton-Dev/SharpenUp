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

        #endregion

        #region Public Status Pages

        #endregion
    }
}
