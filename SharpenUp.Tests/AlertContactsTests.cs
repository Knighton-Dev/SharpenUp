using System;
using System.Threading.Tasks;
using SharpenUp.Client;
using SharpenUp.Common;
using SharpenUp.Common.Models.Alerts;
using SharpenUp.Common.Types;
using Xunit;

namespace SharpenUp.Tests
{
    public class AlertContactsTests
    {
        private readonly IUptimeManager _goodManager;
        private readonly IUptimeManager _badManager;

        public AlertContactsTests()
        {
            _goodManager = new UptimeManager( Environment.GetEnvironmentVariable( "GOOD_API_KEY" ) );
            _badManager = new UptimeManager( "thisKeyIsBad" );
        }

        [Fact]
        public async Task AllAlertContacts_GoodKey()
        {
            AlertContactsResult result = await _goodManager.GetAlertContactsAsync();

            Assert.Equal( RequestStatusType.ok, result.Status );
            Assert.Null( result.Error );
            Assert.Equal( 0, result.Offset );
            Assert.Equal( 50, result.Limit );
            Assert.True( result.Total > 0 );
            Assert.NotNull( result.AlertContacts );
            Assert.Equal( AlertContactType.Email, result.AlertContacts[ 0 ].Type );
            Assert.Equal( AlertContactStatusType.Active, result.AlertContacts[ 0 ].Status );
        }

        [Fact]
        public async Task AllAlertContacts_BadKey()
        {
            AlertContactsResult result = await _badManager.GetAlertContactsAsync();

            Assert.Equal( RequestStatusType.fail, result.Status );
            Assert.NotNull( result.Error );
            Assert.Null( result.AlertContacts );
        }
    }
}
