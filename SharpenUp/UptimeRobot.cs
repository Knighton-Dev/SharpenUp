using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;
using SharpenUp.Requests;
using SharpenUp.Results;
using System.Collections.Generic;
using SharpenUp.Models;
using System.Web;

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
            IRestResponse response = await GetRestResponseAsync( "getAccountDetails", $"api_key={_apiKey}&format=json" );

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

            if ( request.IncludeSSL )
            {
                queryString.Append( "&ssl=1" );
            }

            // TODO: Figure these out. I don't have a "pro" plan
            //if ( request.CustomHttpHeaders )
            //{
            //    queryString.Append( "&custom_http_headers=1" );
            //}

            //if ( request.CustomHttpStatuses )
            //{
            //    queryString.Append( "&custom_http_statuses=1" );
            //}

            if ( request.Timezone )
            {
                queryString.Append( "&timezone=1" );
            }

            if ( request.Offset > 0 )
            {
                queryString.Append( $"&offset={request.Offset}" );
            }

            if ( request.Limit < 50 )
            {
                queryString.Append( $"&limit={request.Limit}" );
            }

            if ( !string.IsNullOrEmpty( request.Search ) )
            {
                queryString.Append( $"&search={HttpUtility.UrlEncode( request.Search )}" );
            }

            IRestResponse response = await GetRestResponseAsync( "getMonitors", queryString.ToString() );

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
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<AlertContactsResult> GetAlertContactsAsync( int id )
        {
            AlertContactsRequest alertContactsRequest = new AlertContactsRequest
            {
                AlertContacts = new List<int> { id }
            };

            return await GetAlertContactsAsync( alertContactsRequest );
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

            IRestResponse response = await GetRestResponseAsync( "getAlertContacts", queryString.ToString() );

            return JsonConvert.DeserializeObject<AlertContactsResult>( response.Content );
        }

        /// <summary>
        /// New alert contacts of any type (mobile/SMS alert contacts are not supported yet) can be created using this method.
        /// The alert contacts created using the API are validated with the same way as they were created from uptimerobot.com (activation link for e-mails, etc.).
        /// </summary>
        /// <param name="contact"></param>
        /// <returns></returns>
        public async Task<AlertContactsResult> CreateAlertContactAsync( ContactType contactType, string contactValue, string friendlyName )
        {
            if ( !string.IsNullOrWhiteSpace( friendlyName ) && !string.IsNullOrWhiteSpace( contactValue ) && contactType != ContactType.SMS )
            {
                StringBuilder queryString = new StringBuilder( $"api_key={_apiKey}&format=json" );

                queryString.Append( $"&type={(int)contactType}" );
                queryString.Append( $"&value={HttpUtility.HtmlEncode( contactValue )}" );
                queryString.Append( $"&friendly_name={HttpUtility.HtmlEncode( friendlyName )}" );

                IRestResponse response = await GetRestResponseAsync( "newAlertContact", queryString.ToString() );

                return JsonConvert.DeserializeObject<AlertContactsResult>( response.Content );
            }

            return null;
        }

        /// <summary>
        /// Alert contacts can be edited using this method.
        /// </summary>
        /// <returns></returns>
        public async Task<AlertContactsResult> UpdateAlertContactAsync( int alertContactId, string friendlyName, string contactValue )
        {
            AlertContactsResult existingContact = await GetAlertContactsAsync( alertContactId );

            if ( existingContact.AlertContacts?.Count > 0 )
            {
                StringBuilder queryString = new StringBuilder( $"api_key={_apiKey}&format=json" );

                if ( !string.IsNullOrWhiteSpace( friendlyName ) )
                {
                    queryString.Append( $"&id={alertContactId}" );
                    queryString.Append( $"&friendly_name={HttpUtility.HtmlEncode( friendlyName )}" );

                    if ( existingContact.AlertContacts[ 0 ].ContactType == ContactType.WebHook && !string.IsNullOrWhiteSpace( contactValue ) )
                    {
                        queryString.Append( $"&value={HttpUtility.HtmlEncode( contactValue )}" );
                    }

                    IRestResponse response = await GetRestResponseAsync( "editAlertContact", queryString.ToString() );

                    return JsonConvert.DeserializeObject<AlertContactsResult>( response.Content );
                }
            }

            return null;
        }

        /// <summary>
        /// Alert contacts can be deleted using this method.
        /// </summary>
        /// <param name="alertContactId"></param>
        /// <returns></returns>
        public async Task<AlertContactsResult> DeleteAlertContactsAsync( int alertContactId )
        {
            AlertContactsResult existingContact = await GetAlertContactsAsync( alertContactId );

            if ( existingContact.AlertContacts?.Count > 0 )
            {
                StringBuilder queryString = new StringBuilder( $"api_key={_apiKey}&format=json" );

                queryString.Append( $"&id={alertContactId}" );

                IRestResponse response = await GetRestResponseAsync( "deleteAlertContact", queryString.ToString() );

                return JsonConvert.DeserializeObject<AlertContactsResult>( response.Content );
            }

            return null;
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

            IRestResponse response = await GetRestResponseAsync( "getPSPs", queryString.ToString() );

            return JsonConvert.DeserializeObject<PublicStatusPageResult>( response.Content );
        }

        /// <summary>
        /// New public status pages can be created using this method.
        /// </summary>
        /// <param name="friendlyName">Required</param>
        /// <param name="monitors">required (The monitors to be displayed can be sent as 15830-32696-83920. Or 0 for displaying all monitors)</param>
        /// <returns></returns>
        public async Task<PublicStatusPageResult> CreatePublicStatusPageAsync( string friendlyName, List<int> monitors )
        {
            return await CreatePublicStatusPageAsync( friendlyName, monitors, "", "", PublicStatusPageSort.FriendlyNameAscending, false, PublicStatusPageStatus.Active );
        }

        /// <summary>
        /// New public status pages can be created using this method.
        /// </summary>
        /// <param name="friendlyName">Required</param>
        /// <param name="monitors">Required (The monitors to be displayed can be sent as 15830-32696-83920. Or 0 for displaying all monitors)</param>
        /// <param name="customDomain">Optional</param>
        /// <param name="password">Optional</param>
        /// <param name="sort">Optional</param>
        /// <param name="hideUrlLinks">Optional (for hiding the Uptime Robot links and only available in the Pro Plan)</param>
        /// <param name="status">Optional</param>
        /// <returns></returns>
        public async Task<PublicStatusPageResult> CreatePublicStatusPageAsync( string friendlyName, List<int> monitors, string customDomain, string password, PublicStatusPageSort sort, bool hideUrlLinks, PublicStatusPageStatus status )
        {
            if ( !string.IsNullOrWhiteSpace( friendlyName ) )
            {
                StringBuilder queryString = new StringBuilder( $"api_key={_apiKey}&format=json" );

                queryString.Append( $"&friendly_name={HttpUtility.HtmlEncode( friendlyName )}" );

                if ( monitors?.Count > 0 )
                {
                    queryString.Append( "&monitors=" );
                    queryString.Append( string.Join( "-", monitors ) );
                }
                else
                {
                    queryString.Append( "&monitors=0" );
                }

                if ( !string.IsNullOrWhiteSpace( customDomain ) )
                {
                    queryString.Append( $"&custom_domain={HttpUtility.HtmlEncode( customDomain )}" );
                }

                if ( !string.IsNullOrWhiteSpace( password ) )
                {
                    queryString.Append( $"&password={HttpUtility.HtmlEncode( password )}" );
                }

                queryString.Append( $"&sort={(int)sort}" );
                queryString.Append( $"&hide_url_links={hideUrlLinks}" );
                queryString.Append( $"&status={(int)status}" );

                IRestResponse response = await GetRestResponseAsync( "newPSP", queryString.ToString() );

                return JsonConvert.DeserializeObject<PublicStatusPageResult>( response.Content );
            }

            return null;
        }

        #endregion

        /// <summary>
        /// Makes reusing the RestSharp logic a little easier. 
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        private async Task<IRestResponse> GetRestResponseAsync( string endpoint, string query )
        {
            RestClient restClient = new RestClient( $"https://api.uptimerobot.com/v2/{endpoint}" );
            RestRequest restRequest = new RestRequest( Method.POST );

            restRequest.AddHeader( "content-type", "application/x-www-form-urlencoded" );
            restRequest.AddHeader( "cache-control", "no-cache" );

            restRequest.AddParameter( "application/x-www-form-urlencoded", query, ParameterType.RequestBody );

            return await restClient.ExecuteAsync( restRequest );
        }
    }
}
