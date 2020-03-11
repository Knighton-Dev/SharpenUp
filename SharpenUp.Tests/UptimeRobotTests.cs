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
            Assert.True( result.Limit.HasValue );

            // Offset
            Assert.True( result.Offset.HasValue );

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
            Assert.True( result.Limit.HasValue );

            // Offset
            Assert.True( result.Offset.HasValue );

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

        #endregion

        #region Maintenance Windows

        #endregion

        #region Public Status Pages

        #endregion
    }
}
