using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using RestSharp;
using SharpenUp.Models;
using SharpenUp.Requests;
using SharpenUp.Results;

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
            try
            {
                IRestResponse response = await GetRestResponseAsync( "getAccountDetails", $"api_key={_apiKey}&format=json" );

                return JsonConvert.DeserializeObject<AccountDetailsResult>( response.Content );
            }
            catch ( Exception e )
            {
                return new AccountDetailsResult
                {
                    Status = Status.fail,
                    Error = new Error
                    {
                        Type = "Internal Exception",
                        Message = e.Message
                    }
                };
            }
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
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<MonitorsResult> GetMonitorsAsync( int id )
        {
            MonitorsRequest request = new MonitorsRequest { Monitors = new List<int> { id } };

            return await GetMonitorsAsync( request );
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
            try
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

                if ( request.CustomUptimeRanges?.Count > 0 )
                {
                    List<Tuple<double, double>> convertedDates = new List<Tuple<double, double>>();
                    List<string> joinedRanges = new List<string>();

                    foreach ( Tuple<DateTime, DateTime> range in request.CustomUptimeRanges )
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

                    if ( request.LogsStartDate != DateTime.MinValue )
                    {
                        queryString.Append( $"&logs_start_date={ConvertDateTimeToSeconds( request.LogsStartDate )}" );
                    }

                    if ( request.LogsEndDate != DateTime.MaxValue )
                    {
                        queryString.Append( $"&logs_end_date={ConvertDateTimeToSeconds( request.LogsEndDate )}" );
                    }

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

                    if ( request.ResponseTimesAverage.HasValue )
                    {
                        queryString.Append( $"&response_times_average={request.ResponseTimesAverage.Value}" );
                    }

                    if ( request.ResponseTimesStartDate.HasValue && request.ResponseTimesEndDate.HasValue )
                    {
                        TimeSpan timeSpan = request.ResponseTimesEndDate.Value - request.ResponseTimesStartDate.Value;

                        if ( timeSpan.TotalDays < 7 )
                        {
                            queryString.Append( $"&response_times_start_date={ConvertDateTimeToSeconds( request.ResponseTimesStartDate.Value )}" );
                            queryString.Append( $"&response_times_end_date={ConvertDateTimeToSeconds( request.ResponseTimesEndDate.Value )}" );
                        }
                        else
                        {
                            throw new Exception( "Start and End Date can not be more than 7 days apart." );
                        }
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
                if ( request.CustomHttpHeaders )
                {
                    throw new NotImplementedException( "Not currently implemented." );
                }

                if ( request.CustomHttpStatuses )
                {
                    throw new NotImplementedException( "Not currently implemented." );
                }

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
            catch ( Exception e )
            {
                return new MonitorsResult
                {
                    Status = Status.fail,
                    Error = new Error
                    {
                        Type = "Internal Exception",
                        Message = e.Message
                    }
                };
            }
        }

        /// <summary>
        /// New monitors of any type can be created using this method.
        /// NOT IMPLEMENTED
        /// </summary>
        /// <param name="friendlyName"></param>
        /// <param name="Url"></param>
        /// <param name="monitorType"></param>
        /// <returns></returns>
        private async Task<MonitorsResult> CreateMonitorAsync( string friendlyName, string Url,
            MonitorType type, MonitorSubType subType, int? port, KeywordType keywordType, string keywordValue,
            int? interval, string username, string password, string method, PostType postType,
            string postValue, PostContentType postContentType, List<AlertContact> alertContacts,
            List<MaintenanceWindow> maintenanceWindows, bool ignoreSSLErrors )
        {
            try
            {
                StringBuilder queryString = new StringBuilder( $"api_key={_apiKey}&format=json" );

                if ( !CheckString( friendlyName ) )
                {
                    queryString.Append( $"&friendly_name={HttpUtility.HtmlEncode( friendlyName )}" );
                }
                else
                {
                    throw new Exception( "Friendly Name is Required" );
                }

                if ( !CheckString( Url ) )
                {
                    queryString.Append( $"&url={HttpUtility.HtmlEncode( Url )}" );
                }
                else
                {
                    throw new Exception( "URL is Required" );
                }

                queryString.Append( $"&type={(int)type}" );

                if ( type == MonitorType.Port )
                {
                    if ( port.HasValue )
                    {
                        queryString.Append( $"&sub_type={(int)subType}" );
                        queryString.Append( $"&port={port.Value}" );
                    }
                    else
                    {
                        throw new Exception( "Port is required for Port Monitoring" );
                    }
                }
                else if ( type == MonitorType.Keyword )
                {
                    if ( !CheckString( keywordValue ) )
                    {
                        queryString.Append( $"&keyword_type={(int)keywordType}" );
                        queryString.Append( $"&keyword_valye={keywordValue}" );
                    }
                    else
                    {
                        throw new Exception( "Keyword Value is required for Keyword Monitoring" );
                    }
                }

                if ( interval.HasValue )
                {
                    queryString.Append( $"&interval={interval.Value}" );
                }

                if ( !CheckString( username ) && !CheckString( password ) )
                {
                    queryString.Append( $"&http_username={username}" );
                    queryString.Append( $"&http_password={password}" );
                }

                IRestResponse response = await GetRestResponseAsync( "newMonitor", queryString.ToString() );

                return JsonConvert.DeserializeObject<MonitorsResult>( response.Content );
            }
            catch ( Exception e )
            {
                return new MonitorsResult
                {
                    Status = Status.fail,
                    Error = new Error
                    {
                        Type = "Inner Exception",
                        Message = e.Message
                    }
                };
            }
        }

        /// <summary>
        /// Monitors can be edited using this method.
        /// Important: The type of a monitor can not be edited (like changing a HTTP monitor into a Port monitor). For such cases, deleting the monitor and re-creating a new one is adviced.
        /// NOT IMPLEMENTED
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<MonitorsResult> EditMonitorAsync( int id )
        {
            try
            {
                throw new NotImplementedException( "Not yet implemented" );
            }
            catch ( Exception e )
            {
                return new MonitorsResult
                {
                    Status = Status.fail,
                    Error = new Error
                    {
                        Type = "Inner Exception",
                        Message = e.Message
                    }
                };
            }
        }

        /// <summary>
        /// Monitors can be deleted using this method.
        /// </summary>
        /// <param name="monitorId"></param>
        /// <returns></returns>
        public async Task<MonitorsResult> DeleteMonitorAsync( int monitorId )
        {
            try
            {
                MonitorsResult existingMonitor = await GetMonitorsAsync( monitorId );

                if ( existingMonitor.Monitors?.Count > 0 )
                {
                    StringBuilder queryString = new StringBuilder( $"api_key={_apiKey}&format=json" );

                    queryString.Append( $"&id={monitorId}" );

                    IRestResponse response = await GetRestResponseAsync( "deleteMonitor", queryString.ToString() );

                    return JsonConvert.DeserializeObject<MonitorsResult>( response.Content );
                }
                else
                {
                    throw new Exception( "Monitor Not Found" );
                }
            }
            catch ( Exception e )
            {
                return new MonitorsResult
                {
                    Status = Status.fail,
                    Error = new Error
                    {
                        Type = "Inner Exception",
                        Message = e.Message
                    }
                };
            }
        }

        /// <summary>
        /// Monitors can be reset (deleting all stats and response time data) using this method.
        /// </summary>
        /// <param name="monitorId"></param>
        /// <returns></returns>
        public async Task<MonitorsResult> ResetMonitorAsync( int monitorId )
        {
            try
            {
                MonitorsResult existingMonitor = await GetMonitorsAsync( monitorId );

                if ( existingMonitor.Monitors?.Count > 0 )
                {
                    StringBuilder queryString = new StringBuilder( $"api_key={_apiKey}&format=json" );

                    queryString.Append( $"&id={monitorId}" );

                    IRestResponse response = await GetRestResponseAsync( "resetMonitor", queryString.ToString() );

                    return JsonConvert.DeserializeObject<MonitorsResult>( response.Content );
                }
                else
                {
                    throw new Exception( "Monitor Not Found" );
                }
            }
            catch ( Exception e )
            {
                return new MonitorsResult
                {
                    Status = Status.fail,
                    Error = new Error
                    {
                        Type = "Inner Exception",
                        Message = e.Message
                    }
                };
            }
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
            try
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
            catch ( Exception e )
            {
                return new AlertContactsResult
                {
                    Status = Status.fail,
                    Error = new Error
                    {
                        Type = "Inner Exception",
                        Message = e.Message
                    }
                };
            }
        }

        /// <summary>
        /// New alert contacts of any type (mobile/SMS alert contacts are not supported yet) can be created using this method.
        /// The alert contacts created using the API are validated with the same way as they were created from uptimerobot.com (activation link for e-mails, etc.).
        /// </summary>
        /// <param name="contact"></param>
        /// <returns></returns>
        public async Task<AlertContactsResult> CreateAlertContactAsync( ContactType contactType, string contactValue, string friendlyName )
        {
            try
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
                else
                {
                    throw new Exception( "Incorrent Parameters" );
                }
            }
            catch ( Exception e )
            {
                return new AlertContactsResult
                {
                    Status = Status.fail,
                    Error = new Error
                    {
                        Type = "Inner Exception",
                        Message = e.Message
                    }
                };
            }
        }

        /// <summary>
        /// Alert contacts can be edited using this method.
        /// </summary>
        /// <returns></returns>
        public async Task<AlertContactsResult> UpdateAlertContactAsync( int alertContactId, string friendlyName, string contactValue )
        {
            try
            {
                AlertContactsResult existingContact = await GetAlertContactsAsync( alertContactId );

                if ( existingContact.AlertContacts?.Count > 0 )
                {
                    StringBuilder queryString = new StringBuilder( $"api_key={_apiKey}&format=json" );

                    queryString.Append( $"&id={alertContactId}" );
                    queryString.Append( $"&friendly_name={HttpUtility.HtmlEncode( friendlyName )}" );

                    if ( existingContact.AlertContacts[ 0 ].ContactType == ContactType.WebHook && !string.IsNullOrWhiteSpace( contactValue ) )
                    {
                        queryString.Append( $"&value={HttpUtility.HtmlEncode( contactValue )}" );
                    }

                    IRestResponse response = await GetRestResponseAsync( "editAlertContact", queryString.ToString() );

                    return JsonConvert.DeserializeObject<AlertContactsResult>( response.Content );
                }
                else
                {
                    throw new Exception( "Contact does not exist." );
                }
            }
            catch ( Exception e )
            {
                return new AlertContactsResult
                {
                    Status = Status.fail,
                    Error = new Error
                    {
                        Type = "Inner Exception",
                        Message = e.Message
                    }
                };
            }
        }

        /// <summary>
        /// Alert contacts can be deleted using this method.
        /// </summary>
        /// <param name="alertContactId"></param>
        /// <returns></returns>
        public async Task<AlertContactsResult> DeleteAlertContactsAsync( int alertContactId )
        {
            try
            {
                AlertContactsResult existingContact = await GetAlertContactsAsync( alertContactId );

                if ( existingContact.AlertContacts?.Count > 0 )
                {
                    StringBuilder queryString = new StringBuilder( $"api_key={_apiKey}&format=json" );

                    queryString.Append( $"&id={alertContactId}" );

                    IRestResponse response = await GetRestResponseAsync( "deleteAlertContact", queryString.ToString() );

                    return JsonConvert.DeserializeObject<AlertContactsResult>( response.Content );
                }
                else
                {
                    throw new Exception( "No Alert Contact Found" );
                }
            }
            catch ( Exception e )
            {
                return new AlertContactsResult
                {
                    Status = Status.fail,
                    Error = new Error
                    {
                        Type = "Inner Exception",
                        Message = e.Message
                    }
                };
            }
        }

        #endregion

        #region Maintenance Windows

        public async Task<MaintenanceWindowsResult> GetMaintenanceWindowsAsync()
        {
            return await GetMaintenanceWindowsAsync( new MaintenanceWindowsRequest() );
        }

        public async Task<MaintenanceWindowsResult> GetMaintenanceWindowsAsync( MaintenanceWindowsRequest request )
        {
            try
            {
                StringBuilder queryString = new StringBuilder( $"api_key={_apiKey}&format=json" );

                if ( request.MaintenanceWindows?.Count > 0 )
                {
                    queryString.Append( "&alert_contacts=" );
                    queryString.Append( string.Join( "-", request.MaintenanceWindows ) );
                }

                if ( request.Offset != 0 )
                {
                    queryString.Append( $"&offset={request.Offset}" );
                }

                if ( request.Limit != 50 )
                {
                    queryString.Append( $"&limit={request.Limit}" );
                }

                IRestResponse response = await GetRestResponseAsync( "getMWindows", queryString.ToString() );

                return JsonConvert.DeserializeObject<MaintenanceWindowsResult>( response.Content );
            }
            catch ( Exception e )
            {
                return new MaintenanceWindowsResult
                {
                    Status = Status.fail,
                    Error = new Error
                    {
                        Type = "Inner Exception",
                        Message = e.Message
                    }
                };
            }
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
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<PublicStatusPageResult> GetPublicStatusPagesAsync( int id )
        {
            PublicStatusPageRequest request = new PublicStatusPageRequest { PublicStatusPages = new List<int> { id } };

            return await GetPublicStatusPagesAsync( request );
        }

        /// <summary>
        /// The list of public status pages can be called with this method.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<PublicStatusPageResult> GetPublicStatusPagesAsync( PublicStatusPageRequest request )
        {
            try
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
            catch ( Exception e )
            {
                return new PublicStatusPageResult
                {
                    Status = Status.fail,
                    Error = new Error
                    {
                        Type = "Inner Exception",
                        Message = e.Message
                    }
                };
            }
        }

        /// <summary>
        /// New public status pages can be created using this method.
        /// </summary>
        /// <param name="friendlyName">Required</param>
        /// <param name="monitors">required (The monitors to be displayed can be sent as 15830-32696-83920. Or 0 for displaying all monitors)</param>
        /// <returns></returns>
        public async Task<PublicStatusPageResult> CreatePublicStatusPageAsync( string friendlyName, List<int> monitors )
        {
            return await CreatePublicStatusPageAsync( friendlyName, monitors, "", "", PublicStatusPageSort.FriendlyNameAscending );
        }

        /// <summary>
        /// New public status pages can be created using this method.
        /// </summary>
        /// <param name="friendlyName">Required</param>
        /// <param name="monitors">Required (The monitors to be displayed can be sent as 15830-32696-83920. Or 0 for displaying all monitors)</param>
        /// <param name="customDomain">Optional</param>
        /// <param name="password">Optional</param>
        /// <param name="sort">Optional</param>
        /// <returns></returns>
        public async Task<PublicStatusPageResult> CreatePublicStatusPageAsync( string friendlyName, List<int> monitors, string customDomain, string password, PublicStatusPageSort sort )
        {
            try
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

                    IRestResponse response = await GetRestResponseAsync( "newPSP", queryString.ToString() );

                    return JsonConvert.DeserializeObject<PublicStatusPageResult>( response.Content );
                }
                else
                {
                    throw new Exception( "A Friendly Name is Required" );
                }
            }
            catch ( Exception e )
            {
                return new PublicStatusPageResult
                {
                    Status = Status.fail,
                    Error = new Error
                    {
                        Type = "Inner Exception",
                        Message = e.Message
                    }
                };
            }
        }

        /// <summary>
        /// Public status pages can be edited using this method.
        /// </summary>
        /// <param name="id">Required</param>
        /// <param name="friendlyName">Optional</param>
        /// <param name="monitors">Optional</param>
        /// <param name="customDomain">Optional</param>
        /// <param name="password">Optional</param>
        /// <param name="sort">Optional</param>
        /// <returns></returns>
        public async Task<PublicStatusPageResult> UpdatePublicStatusPageAsync( int id, string friendlyName, List<int> monitors, string customDomain, string password, PublicStatusPageSort sort )
        {
            try
            {
                PublicStatusPageResult existingPublicPage = await GetPublicStatusPagesAsync( id );

                if ( existingPublicPage.PublicStatusPages?.Count > 0 )
                {
                    StringBuilder queryString = new StringBuilder( $"api_key={_apiKey}&format=json" );

                    queryString.Append( $"&id={id}" );

                    if ( !existingPublicPage.PublicStatusPages[ 0 ].FriendlyName.Equals( friendlyName ) )
                    {
                        queryString.Append( $"&friendly_name={HttpUtility.HtmlEncode( friendlyName )}" );
                    }

                    if ( monitors?.Count > 0 )
                    {
                        queryString.Append( "&monitors=" );
                        queryString.Append( string.Join( "-", monitors ) );
                    }
                    else
                    {
                        queryString.Append( "&monitors=0" );
                    }

                    if ( !existingPublicPage.PublicStatusPages[ 0 ].CustomDomain.Equals( customDomain ) )
                    {
                        queryString.Append( $"&custom_domain={HttpUtility.HtmlEncode( customDomain )}" );
                    }

                    if ( !string.IsNullOrWhiteSpace( existingPublicPage.PublicStatusPages[ 0 ].Password ) && !existingPublicPage.PublicStatusPages[ 0 ].Password.Equals( password ) )
                    {
                        queryString.Append( $"&password={HttpUtility.HtmlEncode( password )}" );
                    }

                    queryString.Append( $"&sort={(int)sort}" );

                    IRestResponse response = await GetRestResponseAsync( "editPSP", queryString.ToString() );

                    return JsonConvert.DeserializeObject<PublicStatusPageResult>( response.Content );
                }
                else
                {
                    throw new Exception( "No Public Status Page was found!" );
                }
            }
            catch ( Exception e )
            {
                return new PublicStatusPageResult
                {
                    Status = Status.fail,
                    Error = new Error
                    {
                        Type = "Inner Exception",
                        Message = e.Message
                    }
                };
            }
        }

        /// <summary>
        /// Public status pages can be deleted using this method.
        /// </summary>
        /// <param name="publicStatusPageId">Required</param>
        /// <returns></returns>
        public async Task<PublicStatusPageResult> DeletePublicStatusPageAsync( int publicStatusPageId )
        {
            try
            {
                PublicStatusPageResult existingPublicPage = await GetPublicStatusPagesAsync( publicStatusPageId );

                if ( existingPublicPage.PublicStatusPages?.Count > 0 )
                {
                    StringBuilder queryString = new StringBuilder( $"api_key={_apiKey}&format=json" );

                    queryString.Append( $"&id={publicStatusPageId}" );

                    IRestResponse response = await GetRestResponseAsync( "deletePSP", queryString.ToString() );

                    return JsonConvert.DeserializeObject<PublicStatusPageResult>( response.Content );
                }
                else
                {
                    throw new Exception( "No Public Status Page was found!" );
                }
            }
            catch ( Exception e )
            {
                return new PublicStatusPageResult
                {
                    Status = Status.fail,
                    Error = new Error
                    {
                        Type = "Inner Exception",
                        Message = e.Message
                    }
                };
            }
        }

        #endregion

        #region Private Methods

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

        /// <summary>
        /// Converts a DateTime to Unix time. 
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        private double ConvertDateTimeToSeconds( DateTime date )
        {
            TimeSpan span = date.Subtract( new DateTime( 1970, 1, 1, 0, 0, 0, DateTimeKind.Utc ) );
            return span.TotalSeconds;
        }

        private bool CheckString( string value )
        {
            return string.IsNullOrWhiteSpace( value );
        }

        #endregion
    }
}
