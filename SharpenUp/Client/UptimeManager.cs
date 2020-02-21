using System;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;
using SharpenUp.Common;
using SharpenUp.Common.Models.Accounts;
using SharpenUp.Common.Models.Alerts;
using SharpenUp.Common.Models.Monitors;

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

        public async Task<AlertContactsResult> GetAlertContactsAsync()
        {
            try
            {
                RestClient client = new RestClient( "https://api.uptimerobot.com/v2/getAlertContacts" );
                RestRequest request = new RestRequest( Method.POST );

                request.AddHeader( "content-type", "application/x-www-form-urlencoded" );
                request.AddHeader( "cache-control", "no-cache" );

                request.AddParameter( "application/x-www-form-urlencoded", $"api_key={_apiKey}&format=json", ParameterType.RequestBody );

                IRestResponse response = await client.ExecuteAsync( request );

                return JsonConvert.DeserializeObject<AlertContactsResult>( response.Content );
            }
            catch ( Exception e )
            {
                throw e;
            }
        }

        public async Task<MonitorsResult> GetMonitorsAsync()
        {
            return await GetMonitorsAsync( new MonitorsRequest() );
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
                if ( request.StatusTypes?.Count > 0 )
                {
                    throw new NotImplementedException();
                }

                // UptimeDateRanges
                if ( request.UptimeDateRanges[ 0 ].Item1 != DateTime.MinValue )
                {
                    throw new NotImplementedException();
                }

                // IncludeAllTimeUptimeRatio
                if ( request.IncludeAllTimeUptimeRatio )
                {
                    queryString.Append( "&all_time_uptime_ratio=1" );
                }

                // IncludeAllTimeUptimeDurations
                if ( request.IncludeAllTimeUptimeDurations )
                {
                    throw new NotImplementedException();
                }

                // Include Logs
                if ( request.IncludeLogs )
                {
                    queryString.Append( "&logs=1" );
                }

                // LogStartDate
                if ( request.LogsStartDate != DateTime.MinValue )
                {
                    throw new NotImplementedException();
                }

                // LogEndDate
                if ( request.LogsEndDate != DateTime.MaxValue )
                {
                    throw new NotImplementedException();
                }

                // LogsLimit
                if ( request.LogsLimit != 50 )
                {
                    throw new NotImplementedException();
                }

                // IncludeResponseTimes
                if ( request.IncludeResponseTimes )
                {
                    throw new NotImplementedException();
                }

                // ResponseTimesStartDate
                if ( request.ResponseTimesStartDate != DateTime.MinValue )
                {
                    throw new NotImplementedException();
                }

                // ResponseTimesEndDate
                if ( request.ResponseTimesEndDate != DateTime.MaxValue )
                {
                    throw new NotImplementedException();
                }

                // IncludeAlertContacts
                if ( request.IncludeAlertContacts )
                {
                    queryString.Append( "&alert_contacts=1" );
                }

                // IncludeMaintenanceWindows
                if ( request.IncludeMaintenanceWindows )
                {
                    throw new NotImplementedException();
                }

                // IncludeCustomHTTPHeaders
                if ( request.IncludeCustomHttpHeaders )
                {
                    throw new NotImplementedException();
                }

                // IncludeCustomHttpStatus
                if ( request.IncludeCustomHttpStatus )
                {
                    throw new NotImplementedException();
                }

                // IncludeTimezone
                if ( request.IncludeTimezone )
                {
                    queryString.Append( "&timezone=1" );
                }

                // Offset
                if ( request.Offset != 0 )
                {
                    throw new NotImplementedException();
                }

                // Limit
                if ( request.Limit != 50 )
                {
                    throw new NotImplementedException();
                }

                // SearchTerm
                if ( !string.IsNullOrWhiteSpace( request.SearchTerm ) )
                {
                    throw new NotImplementedException();
                }

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
