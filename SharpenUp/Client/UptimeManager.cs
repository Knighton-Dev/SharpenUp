using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;
using SharpenUp.Common;
using SharpenUp.Common.Models;
using System.Collections.Generic;
using System.Text;

namespace SharpenUp.Client
{
    public class UptimeManager : IUptimeManager
    {
        private readonly string _apiKey;

        public UptimeManager( string apiKey )
        {
            _apiKey = apiKey;
        }

        public async Task<AccountDetailsResult> GetAccountDetailsAsync()
        {
            try
            {
                RestClient client = new RestClient( "https://api.uptimerobot.com/v2/getAccountDetails" );
                RestRequest request = new RestRequest( Method.POST );

                request.AddHeader( "content-type", "application/x-www-form-urlencoded" );
                request.AddHeader( "cache-control", "no-cache" );

                request.AddParameter( "application/x-www-form-urlencoded", $"api_key={_apiKey}&format=json", ParameterType.RequestBody );

                IRestResponse response = await client.ExecuteAsync( request );

                return JsonConvert.DeserializeObject<AccountDetailsResult>( response.Content );
            }
            catch ( Exception e )
            {
                throw e;
            }
        }

        public async Task<MonitorsResult> GetMonitorAsync( int monitorId, bool includeLogs = false )
        {
            List<int> monitors = new List<int>
            {
                monitorId
            };

            return await GetMonitorsAsync( monitors, includeLogs );
        }

        public async Task<MonitorsResult> GetMonitorsAsync( bool includeLogs = false )
        {
            return await GetMonitorsAsync( null, includeLogs );
        }

        public async Task<MonitorsResult> GetMonitorsAsync( List<int> monitorIds, bool includeLogs = false )
        {
            try
            {
                StringBuilder queryString = new StringBuilder( $"api_key={_apiKey}&format=json" );

                if ( monitorIds?.Count > 0 )
                {
                    queryString.Append( "&monitors=" );
                    queryString.Append( string.Join( "-", monitorIds ) );
                }

                if ( includeLogs )
                {
                    queryString.Append( "&logs=1" );
                }

                RestClient client = new RestClient( "https://api.uptimerobot.com/v2/getMonitors" );
                RestRequest request = new RestRequest( Method.POST );

                request.AddHeader( "content-type", "application/x-www-form-urlencoded" );
                request.AddHeader( "cache-control", "no-cache" );

                request.AddParameter( "application/x-www-form-urlencoded", queryString.ToString(), ParameterType.RequestBody );

                IRestResponse response = await client.ExecuteAsync( request );

                return JsonConvert.DeserializeObject<MonitorsResult>( response.Content );
            }
            catch ( Exception e )
            {
                throw e;
            }
        }
    }
}
