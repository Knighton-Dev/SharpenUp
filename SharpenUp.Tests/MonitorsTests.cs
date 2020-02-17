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
        public async Task Monitors_GoodKey()
        {
            MonitorsResult result = await _goodManager.GetMonitorsAsync();

            Assert.True( result.Status == StatusType.ok );
            Assert.True( result.Error == null );
        }

        [Fact]
        public async Task Monitors_BadKey()
        {
            MonitorsResult result = await _badManager.GetMonitorsAsync();

            Assert.True( result.Status == StatusType.fail );
            Assert.True( result.Results == null );
        }
    }
}
