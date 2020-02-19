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

        public async Task<MonitorsResult> GetMonitorsAsync( MonitorsRequest request )
        {
            try
            {
                StringBuilder queryString = new StringBuilder( $"api_key={_apiKey}&format=json" );

                #region Query String Modifications

                // Monitor IDs
                if ( request.MonitorIds?.Count > 0 )
                {
                    queryString.Append( "&monitors=" );
                    queryString.Append( string.Join( "-", request.MonitorIds ) );
                }

                // MonitorTypes
                if ( request.MonitorTypes?.Count > 0 )
                {
                    queryString.Append( "&types=" );
                    queryString.Append( string.Join( "-", request.MonitorTypes ) );
                }
                // StatusTypes
                // UptimeDateRanges

                // IncludeAllTimeUptimeRatio
                if ( request.IncludeAllTimeUptimeRatio )
                {
                    queryString.Append( "&all_time_uptime_ratio=1" );
                }

                // IncludeAllTimeUptimeDurations

                // Include Logs
                if ( request.IncludeLogs )
                {
                    queryString.Append( "&logs=1" );
                }

                // LogStartDate
                // LogEndDate
                // LogsLimit
                // IncludeResponseTimes
                // ResponseTimesStartDate
                // ResponseTimesEndDate

                // IncludeAlertContacts
                if ( request.IncludeAlertContacts )
                {
                    queryString.Append( "&alert_contacts=1" );
                }

                // IncludeMaintenanceWindows
                // IncludeCustomHTTPHeaders
                // IncludeCustomHttpStatus

                // IncludeTimezone
                if ( request.IncludeTimezone )
                {
                    queryString.Append( "&timezone=1" );
                }

                // Offset
                // Limit
                // SearchTerm

                // IncludeSLLInfo
                if ( request.IncludeSSLInfo )
                {
                    queryString.Append( "&ssl=1" );
                }

                #endregion

                RestClient restClient = new RestClient( "https://api.uptimerobot.com/v2/getMonitors" );
                RestRequest restRequest = new RestRequest( Method.POST );

                restRequest.AddHeader( "content-type", "application/x-www-form-urlencoded" );
                restRequest.AddHeader( "cache-control", "no-cache" );

                restRequest.AddParameter( "application/x-www-form-urlencoded", queryString.ToString(), ParameterType.RequestBody );

                IRestResponse response = await restClient.ExecuteAsync( restRequest );

                return JsonConvert.DeserializeObject<MonitorsResult>( response.Content );
            }
            catch ( Exception e )
            {
                throw e;
            }
        }
    }
}
