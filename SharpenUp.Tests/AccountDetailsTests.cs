using System;
using System.Threading.Tasks;
using SharpenUp.Client;
using SharpenUp.Common;
using SharpenUp.Common.Models;
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

            Assert.True( accountDetails.Status == RequestStatusType.ok );
            Assert.True( accountDetails.Error == null );
            Assert.True( accountDetails.Account.Email == Environment.GetEnvironmentVariable( "ACCOUNT_EMAIL" ) );
        }

        [Fact]
        public async Task AccountDetails_BadKey()
        {
            AccountDetailsResult accountDetails = await _badManager.GetAccountDetailsAsync();

            Assert.True( accountDetails.Status == RequestStatusType.fail );
            Assert.True( accountDetails.Account == null );
            Assert.True( accountDetails.Error.Type == "invalid_parameter" );
            Assert.True( accountDetails.Error.ParameterName == "api_key" );
            Assert.True( accountDetails.Error.PassedValue == "thisKeyIsBad" );
        }
    }
}
