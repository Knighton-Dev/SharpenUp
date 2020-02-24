using System;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;
using SharpenUp.Common;
using SharpenUp.Common.Models.Accounts;
using SharpenUp.Common.Models.Alerts;
using SharpenUp.Common.Models.Monitors;
using System.Web;
using System.Collections.Generic;

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
                    queryString.Append( "&statuses=" );
                    queryString.Append( string.Join( "-", request.MonitorTypes ) );
                }

                // UptimeDateRanges
                if ( request.UptimeDateRanges[ 0 ].Item1 != DateTime.MinValue )
                {
                    List<Tuple<double, double>> convertedDates = new List<Tuple<double, double>>();
                    List<string> joinedRanges = new List<string>();

                    foreach ( var range in request.UptimeDateRanges )
                    {
                        convertedDates.Add( new Tuple<double, double>( ConvertDateTimeToSeconds( range.Item1 ), ConvertDateTimeToSeconds( range.Item2 ) ) );
                    }

                    foreach ( var dateRange in convertedDates )
                    {
                        joinedRanges.Add( $"{dateRange.Item1}_{dateRange.Item2}" );
                    }

                    queryString.Append( "&custom_uptime_ranges=" );
                    queryString.Append( string.Join( "-", joinedRanges ) );
                }

                // IncludeAllTimeUptimeRatio
                if ( request.IncludeAllTimeUptimeRatio )
                {
                    queryString.Append( "&all_time_uptime_ratio=1" );
                }

                // IncludeAllTimeUptimeDurations
                if ( request.IncludeAllTimeUptimeDurations )
                {
                    queryString.Append( "&all_time_uptime_durations=1" );
                }

                // Include Logs
                if ( request.IncludeLogs )
                {
                    queryString.Append( "&logs=1" );

                    // LogStartDate
                    if ( request.LogsStartDate != DateTime.MinValue )
                    {
                        queryString.Append( $"&logs_start_date={ConvertDateTimeToSeconds( request.LogsStartDate )}" );
                    }

                    // LogEndDate
                    if ( request.LogsEndDate != DateTime.MaxValue )
                    {
                        queryString.Append( $"&logs_end_date={ConvertDateTimeToSeconds( request.LogsEndDate )}" );
                    }

                    // LogsLimit
                    if ( request.LogsLimit != 50 )
                    {
                        queryString.Append( $"&logs_limit={request.LogsLimit}" );
                    }
                }

                // IncludeResponseTimes
                if ( request.IncludeResponseTimes )
                {
                    queryString.Append( "&response_times=1" );

                    // ResponseTimesStartDate
                    if ( request.ResponseTimesStartDate != DateTime.MinValue && request.ResponseTimesEndDate != DateTime.MaxValue )
                    {
                        TimeSpan timeSpan = request.ResponseTimesEndDate - request.ResponseTimesStartDate;
                        if ( timeSpan.TotalDays > 7 )
                        {
                            queryString.Append( $"&response_times_start_date={ConvertDateTimeToSeconds( request.ResponseTimesStartDate )}" );
                            queryString.Append( $"&response_times_end_date={ConvertDateTimeToSeconds( request.ResponseTimesEndDate )}" );
                        }
                        else
                        {
                            throw new Exception( "Difference between the start and end date can not exceed 7 days." );
                        }
                    }
                    else
                    {
                        throw new Exception( "Both Start and End Date must be specified." );
                    }
                }

                // IncludeAlertContacts
                if ( request.IncludeAlertContacts )
                {
                    queryString.Append( "&alert_contacts=1" );
                }

                // IncludeMaintenanceWindows
                if ( request.IncludeMaintenanceWindows )
                {
                    queryString.Append( "&mwindows=1" );
                }

                // IncludeCustomHTTPHeaders
                if ( request.IncludeCustomHttpHeaders )
                {
                    queryString.Append( "&custom_http_headers=1" );
                }

                // IncludeCustomHttpStatus
                if ( request.IncludeCustomHttpStatus )
                {
                    queryString.Append( "&custom_http_statuses=1" );
                }

                // IncludeTimezone
                if ( request.IncludeTimezone )
                {
                    queryString.Append( "&timezone=1" );
                }

                // Offset
                if ( request.PaginationOffset != 0 )
                {
                    queryString.Append( $"&offset={request.PaginationOffset}" );
                }

                // Limit
                if ( request.PaginationLimit != 50 )
                {
                    queryString.Append( $"&liumit={request.PaginationLimit}" );
                }

                // SearchTerm
                if ( !string.IsNullOrWhiteSpace( request.SearchTerm ) )
                {
                    queryString.Append( $"&search={HttpUtility.UrlEncode( request.SearchTerm )}" );
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

        private double ConvertDateTimeToSeconds( DateTime date )
        {
            TimeSpan span = date.Subtract( new DateTime( 1970, 1, 1, 0, 0, 0, DateTimeKind.Utc ) );
            return span.TotalSeconds;
        }
    }
}
