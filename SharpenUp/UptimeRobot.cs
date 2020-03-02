using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;
using SharpenUp.Requests;
using SharpenUp.Results;
using System.Collections.Generic;
using SharpenUp.Models;

namespace SharpenUp
{
    public class UptimeRobot
    {
        private readonly string _apiKey;

        public UptimeRobot( string apiKey )
        {
            _apiKey = apiKey;
        }

        #region Account Details

        /// <summary>
        /// Account details (max number of monitors that can be added and number of up/down/paused monitors) can be grabbed using this method.
        /// </summary>
        /// <returns></returns>
        public async Task<AccountDetailsResult> GetAccountDetailsAsync()
        {
            RestClient restClient = new RestClient( "https://api.uptimerobot.com/v2/getAccountDetails" );
            RestRequest restRequest = new RestRequest( Method.POST );

            restRequest.AddHeader( "content-type", "application/x-www-form-urlencoded" );
            restRequest.AddHeader( "cache-control", "no-cache" );

            restRequest.AddParameter( "application/x-www-form-urlencoded", $"api_key={_apiKey}&format=json", ParameterType.RequestBody );

            IRestResponse response = await restClient.ExecuteAsync( restRequest );

            return JsonConvert.DeserializeObject<AccountDetailsResult>( response.Content );
        }

        #endregion

        #region Monitors

        /// <summary>
        /// This is a Swiss-Army knife type of a method for getting any information on monitors.
        /// By default, it lists all the monitors in a user's account, their friendly names, types (http, keyword, port, etc.), statuses (up, down, etc.) and uptime ratios.
        /// There are optional parameters which lets the getMonitors method to output information on any given monitors rather than all of them.
        /// And also, parameters exist for getting the notification logs( alerts) for each monitor and even which alert contacts were alerted on each notification.
        /// </summary>
        /// <returns></returns>
        public async Task<MonitorsResult> GetMonitorsAsync()
        {
            return await GetMonitorsAsync( new MonitorsRequest() );
        }

        /// <summary>
        /// This is a Swiss-Army knife type of a method for getting any information on monitors.
        /// By default, it lists all the monitors in a user's account, their friendly names, types (http, keyword, port, etc.), statuses (up, down, etc.) and uptime ratios.
        /// There are optional parameters which lets the getMonitors method to output information on any given monitors rather than all of them.
        /// And also, parameters exist for getting the notification logs( alerts) for each monitor and even which alert contacts were alerted on each notification.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<MonitorsResult> GetMonitorsAsync( MonitorsRequest request )
        {
            StringBuilder queryString = new StringBuilder( $"api_key={_apiKey}&format=json" );

            if ( request.Monitors?.Count > 0 )
            {
                queryString.Append( "&monitors=" );
                queryString.Append( string.Join( "-", request.Monitors ) );
            }

            if ( request.MonitorTypes?.Count > 0 )
            {
                List<int> convertToIntegers = new List<int>();

                queryString.Append( "&types=" );

                foreach ( MonitorType monitorType in request.MonitorTypes )
                {
                    convertToIntegers.Add( (int)monitorType );
                }

                queryString.Append( string.Join( "-", convertToIntegers ) );
            }

            if ( request.Statuses?.Count > 0 )
            {
                List<int> convertToIntegers = new List<int>();

                queryString.Append( "&statuses=" );

                foreach ( MonitorStatus monitorType in request.Statuses )
                {
                    convertToIntegers.Add( (int)monitorType );
                }

                queryString.Append( string.Join( "-", convertToIntegers ) );
            }

            if ( request.CustomUptimeRatios?.Count > 0 )
            {
                queryString.Append( "&custom_uptime_ratios=" );
                queryString.Append( string.Join( "-", request.CustomUptimeRatios ) );
            }

            // TODO: Custom Uptime Ranges

            if ( request.AllTimeUptimeRatio )
            {
                queryString.Append( "&all_time_uptime_ratio=1" );
            }

            if ( request.AllTimeUptimeDurations )
            {
                queryString.Append( "&all_time_uptime_durations=1" );
            }

            if ( request.IncludeLogs )
            {
                queryString.Append( "&logs=1" );

                // TODO: Logs Start Date
                // TODO: Logs End Date

                if ( request.LogTypes?.Count > 0 )
                {
                    List<int> convertToIntegers = new List<int>();

                    queryString.Append( "&log_types=" );

                    foreach ( LogType logType in request.LogTypes )
                    {
                        convertToIntegers.Add( (int)logType );
                    }

                    queryString.Append( string.Join( "-", convertToIntegers ) );
                }

                if ( request.LogsLimit.HasValue )
                {
                    queryString.Append( $"&logs_limit={request.LogsLimit.Value}" );
                }
            }

            if ( request.ResponseTimes )
            {
                queryString.Append( "&response_times=1" );

                if ( request.ResponseTimesLimit.HasValue )
                {
                    queryString.Append( $"&response_times_limit={request.ResponseTimesLimit.Value}" );
                }
            }

            if ( request.AlertContacts )
            {
                queryString.Append( "&alert_contacts=1" );
            }

            if ( request.MaintenanceWindows )
            {
                queryString.Append( "&mwindows=1" );
            }

            RestClient restClient = new RestClient( "https://api.uptimerobot.com/v2/getMonitors" );
            RestRequest restRequest = new RestRequest( Method.POST );

            restRequest.AddHeader( "content-type", "application/x-www-form-urlencoded" );
            restRequest.AddHeader( "cache-control", "no-cache" );

            restRequest.AddParameter( "application/x-www-form-urlencoded", queryString.ToString(), ParameterType.RequestBody );

            IRestResponse response = await restClient.ExecuteAsync( restRequest );

            return JsonConvert.DeserializeObject<MonitorsResult>( response.Content );
        }

        #endregion

        #region Alert Contacts

        /// <summary>
        /// The list of alert contacts can be called with this method.
        /// </summary>
        /// <returns></returns>
        public async Task<AlertContactsResult> GetAlertContactsAsync()
        {
            return await GetAlertContactsAsync( new AlertContactsRequest() );
        }

        /// <summary>
        /// The list of alert contacts can be called with this method.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<AlertContactsResult> GetAlertContactsAsync( AlertContactsRequest request )
        {
            StringBuilder queryString = new StringBuilder( $"api_key={_apiKey}&format=json" );

            if ( request.AlertContacts?.Count > 0 )
            {
                queryString.Append( "&alert_contacts=" );
                queryString.Append( string.Join( "-", request.AlertContacts ) );
            }

            if ( request.Offset != 0 )
            {
                queryString.Append( $"&offset={request.Offset}" );
            }

            if ( request.Limit != 50 )
            {
                queryString.Append( $"&limit={request.Limit}" );
            }

            RestClient restClient = new RestClient( "https://api.uptimerobot.com/v2/getAlertContacts" );
            RestRequest restRequest = new RestRequest( Method.POST );

            restRequest.AddHeader( "content-type", "application/x-www-form-urlencoded" );
            restRequest.AddHeader( "cache-control", "no-cache" );

            restRequest.AddParameter( "application/x-www-form-urlencoded", queryString.ToString(), ParameterType.RequestBody );

            IRestResponse response = await restClient.ExecuteAsync( restRequest );

            return JsonConvert.DeserializeObject<AlertContactsResult>( response.Content );
        }

        #endregion

        #region Public Status Pages

        /// <summary>
        /// The list of public status pages can be called with this method.
        /// </summary>
        /// <returns></returns>
        public async Task<PublicStatusPageResult> GetPublicStatusPagesAsync()
        {
            return await GetPublicStatusPagesAsync( new PublicStatusPageRequest() );
        }

        /// <summary>
        /// The list of public status pages can be called with this method.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<PublicStatusPageResult> GetPublicStatusPagesAsync( PublicStatusPageRequest request )
        {
            StringBuilder queryString = new StringBuilder( $"api_key={_apiKey}&format=json" );

            if ( request.PublicStatusPages?.Count > 0 )
            {
                queryString.Append( "&psps=" );
                queryString.Append( string.Join( "-", request.PublicStatusPages ) );
            }

            if ( request.Offset != 0 )
            {
                queryString.Append( $"&offset={request.Offset}" );
            }

            if ( request.Limit != 50 )
            {
                queryString.Append( $"&limit={request.Limit}" );
            }

            RestClient restClient = new RestClient( "https://api.uptimerobot.com/v2/getPSPs" );
            RestRequest restRequest = new RestRequest( Method.POST );

            restRequest.AddHeader( "content-type", "application/x-www-form-urlencoded" );
            restRequest.AddHeader( "cache-control", "no-cache" );

            restRequest.AddParameter( "application/x-www-form-urlencoded", queryString.ToString(), ParameterType.RequestBody );

            IRestResponse response = await restClient.ExecuteAsync( restRequest );

            return JsonConvert.DeserializeObject<PublicStatusPageResult>( response.Content );
        }

        #endregion
    }
}
