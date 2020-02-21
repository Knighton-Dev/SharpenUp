using System;
using System.Threading.Tasks;
using SharpenUp.Client;
using SharpenUp.Common;
using SharpenUp.Common.Models.Accounts;
using SharpenUp.Common.Types;
using Xunit;

namespace SharpenUp.Tests
{
    public class AccountDetailsTests
    {
        private readonly IUptimeManager _goodManager;
        private readonly IUptimeManager _badManager;

        public AccountDetailsTests()
        {
            _goodManager = new UptimeManager( Environment.GetEnvironmentVariable( "GOOD_API_KEY" ) );
            _badManager = new UptimeManager( "thisKeyIsBad" );
        }

        [Fact]
        public async Task AccountDetails_GoodKey()
        {
            AccountDetailsResult accountDetails = await _goodManager.GetAccountDetailsAsync();

            Assert.Equal( RequestStatusType.ok, accountDetails.Status );
            Assert.Null( accountDetails.Error );
            Assert.Equal( Environment.GetEnvironmentVariable( "ACCOUNT_EMAIL" ), accountDetails.Account.Email );
            Assert.Equal( 50, accountDetails.Account.MonitorLimit );
            Assert.Equal( 5, accountDetails.Account.MonitorInterval );
        }

        [Fact]
        public async Task AccountDetails_BadKey()
        {
            AccountDetailsResult accountDetails = await _badManager.GetAccountDetailsAsync();

            Assert.Equal( RequestStatusType.fail, accountDetails.Status );
            Assert.Null( accountDetails.Account );
            Assert.Equal( "invalid_parameter", accountDetails.Error.Type );
            Assert.Equal( "api_key", accountDetails.Error.ParameterName );
            Assert.Equal( "thisKeyIsBad", accountDetails.Error.PassedValue );
        }
    }
}
