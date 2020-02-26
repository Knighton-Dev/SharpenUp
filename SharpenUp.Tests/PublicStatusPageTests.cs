using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SharpenUp.Client;
using SharpenUp.Common;
using SharpenUp.Common.Models.PublicStatusPages;
using SharpenUp.Common.Types;
using Xunit;

namespace SharpenUp.Tests
{
    public class PublicStatusPageTests
    {
        private readonly IUptimeManager _goodManager;
        private readonly IUptimeManager _badManager;

        public PublicStatusPageTests()
        {
            _goodManager = new UptimeManager( Environment.GetEnvironmentVariable( "GOOD_API_KEY" ) );
            _badManager = new UptimeManager( "thisKeyIsBad" );
        }

        [Fact]
        public async Task PublicStatusPages_GoodKey()
        {
            PublicStatusPagesResult result = await _goodManager.GetPublicStatusPagesAsync();

            Assert.Equal( RequestStatusType.ok, result.Status );
            Assert.Equal( Environment.GetEnvironmentVariable( "PSP_NAME_1" ), result.Results[ 0 ].Name );
            Assert.Equal( 1, result.Results[ 0 ].Monitors?.Count );
            Assert.Equal( PublicStatusPageSortType.FriendlyName, result.Results[ 0 ].Sort );
            Assert.Equal( PublicStatusPageStatusType.Active, result.Results[ 0 ].Status );
            Assert.Equal( Environment.GetEnvironmentVariable( "PSP_URL_1" ), result.Results[ 0 ].StandardURL );
            Assert.Equal( "", result.Results[ 0 ].CustomURL );
        }

        [Fact]
        public async Task PublicStatusPages_GoodKey_FilteredIds()
        {
            PublicStatusPagesRequest request = new PublicStatusPagesRequest
            {
                PageIds = new List<int> { 98604 }
            };

            PublicStatusPagesResult result = await _goodManager.GetPublicStatusPagesAsync( request );

            Assert.Equal( RequestStatusType.ok, result.Status );
            Assert.Equal( Environment.GetEnvironmentVariable( "PSP_NAME_1" ), result.Results[ 0 ].Name );
            Assert.Equal( 1, result.Results[ 0 ].Monitors?.Count );
            Assert.Equal( PublicStatusPageSortType.FriendlyName, result.Results[ 0 ].Sort );
            Assert.Equal( PublicStatusPageStatusType.Active, result.Results[ 0 ].Status );
            Assert.Equal( Environment.GetEnvironmentVariable( "PSP_URL_1" ), result.Results[ 0 ].StandardURL );
            Assert.Equal( "", result.Results[ 0 ].CustomURL );
            Assert.Equal( 0, result.Pagination.Offset );
            Assert.Equal( 50, result.Pagination.Limit );
        }

        [Fact]
        public async Task PublicStatusPages_GoodKey_FilteredIds_CustomOffset()
        {
            PublicStatusPagesRequest request = new PublicStatusPagesRequest
            {
                PageIds = new List<int> { 171453 },
                PaginationOffest = 2
            };

            PublicStatusPagesResult result = await _goodManager.GetPublicStatusPagesAsync( request );

            Assert.Equal( RequestStatusType.ok, result.Status );
            Assert.Equal( Environment.GetEnvironmentVariable( "PSP_NAME_2" ), result.Results[ 0 ].Name );
            Assert.Equal( 2, result.Results[ 0 ].Monitors?.Count );
            Assert.Equal( PublicStatusPageSortType.FriendlyName, result.Results[ 0 ].Sort );
            Assert.Equal( PublicStatusPageStatusType.Active, result.Results[ 0 ].Status );
            Assert.Equal( Environment.GetEnvironmentVariable( "PSP_URL_2" ), result.Results[ 0 ].StandardURL );
            Assert.Equal( "", result.Results[ 0 ].CustomURL );
            Assert.Equal( 2, result.Pagination.Offset );
            Assert.Equal( 50, result.Pagination.Limit );
        }

        [Fact]
        public async Task PublicStatusPages_GoodKey_FilteredIds_CustomOffset_CustomLimit()
        {
            PublicStatusPagesRequest request = new PublicStatusPagesRequest
            {
                PageIds = new List<int> { 98604 },
                PaginationOffest = 2,
                PaginationLimit = 15
            };

            PublicStatusPagesResult result = await _goodManager.GetPublicStatusPagesAsync( request );

            Assert.Equal( RequestStatusType.ok, result.Status );
            Assert.Equal( Environment.GetEnvironmentVariable( "PSP_NAME_1" ), result.Results[ 0 ].Name );
            Assert.Equal( 1, result.Results[ 0 ].Monitors?.Count );
            Assert.Equal( PublicStatusPageSortType.FriendlyName, result.Results[ 0 ].Sort );
            Assert.Equal( PublicStatusPageStatusType.Active, result.Results[ 0 ].Status );
            Assert.Equal( Environment.GetEnvironmentVariable( "PSP_URL_1" ), result.Results[ 0 ].StandardURL );
            Assert.Equal( "", result.Results[ 0 ].CustomURL );
            Assert.Equal( 2, result.Pagination.Offset );
            Assert.Equal( 15, result.Pagination.Limit );
        }

        [Fact]
        public async Task PublicStatusPages_BadKey()
        {
            PublicStatusPagesResult result = await _badManager.GetPublicStatusPagesAsync();

            Assert.Equal( RequestStatusType.fail, result.Status );
            Assert.NotNull( result.Error );
        }
    }
}
