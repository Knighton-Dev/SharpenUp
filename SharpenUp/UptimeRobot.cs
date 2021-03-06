﻿using System;
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
        private readonly string _defaultExplanation = "There was an error in processing your request. Please see the message.";

        public UptimeRobot ( string apiKey )
        {
            _apiKey = apiKey;
        }

        #region Account Details

        /// <summary>
        /// Account details (max number of monitors that can be added and number of up/down/paused monitors) can be grabbed using this method.
        /// </summary>
        /// <returns></returns>
        public async Task<AccountDetailsResult> GetAccountDetailsAsync ()
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
        public async Task<MonitorsResult> GetMonitorsAsync ()
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
        public async Task<MonitorsResult> GetMonitorsAsync ( int id )
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
        public async Task<MonitorsResult> GetMonitorsAsync ( MonitorsRequest request )
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

                    if ( request.LogsStartDate.HasValue && request.LogsEndDate.HasValue )
                    {
                        queryString.Append( $"&logs_start_date={(int)ConvertDateTimeToSeconds( request.LogsStartDate.Value )}&logs_end_date={(int)ConvertDateTimeToSeconds( request.LogsEndDate.Value )}" );
                    }
                    else
                    {
                        throw new Exception( "Both the Start and End date must be provided for Logs" );
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

                // TODO: Implement
                if ( request.CustomHttpHeaders )
                {
                    throw new NotImplementedException( "Not currently implemented." );
                }

                // TODO: Implement
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
                        Explanation = "Internal Exception",
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
        public async Task<MonitorsResult> DeleteMonitorAsync ( int monitorId )
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
                        Explanation = "Inner Exception",
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
        public async Task<MonitorsResult> ResetMonitorAsync ( int monitorId )
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
                        Explanation = "Inner Exception",
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
        public async Task<AlertContactsResult> GetAlertContactsAsync ()
        {
            return await GetAlertContactsAsync( new AlertContactsRequest() );
        }

        /// <summary>
        /// The list of alert contacts can be called with this method.
        /// </summary>
        /// <param name="alertContactId"></param>
        /// <returns></returns>
        public async Task<AlertContactsResult> GetAlertContactsAsync ( int alertContactId )
        {
            AlertContactsRequest alertContactsRequest = new AlertContactsRequest
            {
                AlertContacts = new List<int> { alertContactId }
            };

            return await GetAlertContactsAsync( alertContactsRequest );
        }

        /// <summary>
        /// The list of alert contacts can be called with this method.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<AlertContactsResult> GetAlertContactsAsync ( AlertContactsRequest request )
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
        public async Task<AlertContactsResult> CreateAlertContactAsync ( ContactType contactType, string contactValue, string friendlyName )
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
                        Explanation = "Inner Exception",
                        Message = e.Message
                    }
                };
            }
        }

        /// <summary>
        /// Alert contacts can be edited using this method.
        /// </summary>
        /// <returns></returns>
        public async Task<AlertContactsResult> UpdateAlertContactAsync ( int alertContactId, string friendlyName, string contactValue )
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
                        Explanation = "Inner Exception",
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
        public async Task<AlertContactsResult> DeleteAlertContactsAsync ( int alertContactId )
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
                        Explanation = "Inner Exception",
                        Message = e.Message
                    }
                };
            }
        }

        #endregion

        #region Maintenance Windows

        /// <summary>
        /// The list of maintenance windows can be called with this method.
        /// </summary>
        /// <returns></returns>
        public async Task<MaintenanceWindowsResult> GetMaintenanceWindowsAsync ()
        {
            return await GetMaintenanceWindowsAsync( new MaintenanceWindowsRequest() );
        }

        /// <summary>
        /// The list of maintenance windows can be called with this method.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<MaintenanceWindowsResult> GetMaintenanceWindowsAsync ( int id )
        {
            return await GetMaintenanceWindowsAsync( new MaintenanceWindowsRequest { MaintenanceWindows = new List<int> { id } } );
        }

        /// <summary>
        /// The list of maintenance windows can be called with this method.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<MaintenanceWindowsResult> GetMaintenanceWindowsAsync ( MaintenanceWindowsRequest request )
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

        /// <summary>
        /// New maintenance windows can be created using this method.
        /// </summary>
        /// <param name="friendlyName">Required</param>
        /// <param name="maintenanceWindowType">Required</param>
        /// <param name="value">Required (only needed for weekly and monthly maintenance windows and must be sent like 2-4-5 for Tuesday-Thursday-Friday or 10-17-26 for the days of the month)</param>
        /// <param name="startTime">Required</param>
        /// <param name="duration">Required (how many minutes the maintenance window will be active for)</param>
        /// <returns></returns>
        public async Task<MaintenanceWindowsResult> CreateMaintenanceWindowAsync ( string friendlyName, MaintenanceWindowType maintenanceWindowType, string value, TimeSpan startTime, int duration )
        {
            if ( !CheckString( friendlyName ) )
            {
                StringBuilder queryString = new StringBuilder( $"api_key={_apiKey}&format=json" );

                queryString.Append( $"&friendly_name={HttpUtility.HtmlEncode( friendlyName )}" );
                queryString.Append( $"&type={(int)maintenanceWindowType}" );

                if ( maintenanceWindowType == MaintenanceWindowType.Weekly || maintenanceWindowType == MaintenanceWindowType.Monthly )
                {
                    if ( !CheckString( value ) )
                    {
                        queryString.Append( $"&value={HttpUtility.HtmlEncode( value )}" );
                    }
                    else
                    {
                        return MaintenanceWindowsError( ErrorType.MaintenanceWindow_WindowTypeRequiresValue );
                    }
                }

                queryString.Append( $"&duration={duration}" );

                string startTimeString = $"{startTime.Hours}:{startTime.Minutes}";
                queryString.Append( $"&start_time={HttpUtility.HtmlEncode( startTimeString )}" );

                IRestResponse response = await GetRestResponseAsync( "newMWindow", queryString.ToString() );

                return JsonConvert.DeserializeObject<MaintenanceWindowsResult>( response.Content );
            }
            else
            {
                return MaintenanceWindowsError( ErrorType.NoFriendlyName );
            }
        }

        /// <summary>
        /// Maintenance windows can be edited using this method.
        /// </summary>
        /// <param name="id">Required</param>
        /// <param name="friendlyName">Optional</param>
        /// <param name="value">Optional (only needed for weekly and monthly maintenance windows and must be sent like 2-4-5 for Tuesday-Thursday-Friday)</param>
        /// <param name="startTime">Optional (required the start datetime)</param>
        /// <param name="duration">Optional (required how many minutes the maintenance window will be active for)</param>
        /// <returns></returns>
        public async Task<MaintenanceWindowsResult> UpdateMaintenanceWindowAsync ( int id, string friendlyName, string value, TimeSpan startTime, int duration )
        {
            MaintenanceWindowsResult existingMaintenanceWindow = await GetMaintenanceWindowsAsync( id );

            if ( existingMaintenanceWindow.MaintenanceWindows?.Count > 0 )
            {
                StringBuilder queryString = new StringBuilder( $"api_key={_apiKey}&format=json" );

                queryString.Append( $"&id={id}" );

                if ( !existingMaintenanceWindow.MaintenanceWindows[ 0 ].FriendlyName.Equals( friendlyName ) )
                {
                    queryString.Append( $"&friendly_name={HttpUtility.HtmlEncode( friendlyName )}" );
                }

                if ( !existingMaintenanceWindow.MaintenanceWindows[ 0 ].Value.Equals( value ) )
                {
                    queryString.Append( $"&value={HttpUtility.HtmlEncode( value )}" );
                }

                if ( existingMaintenanceWindow.MaintenanceWindows[ 0 ].StartTime != startTime )
                {
                    string startTimeString = $"{startTime.Hours}:{startTime.Minutes}";
                    queryString.Append( $"&start_time={HttpUtility.HtmlEncode( startTimeString )}" );
                }

                if ( existingMaintenanceWindow.MaintenanceWindows[ 0 ].Duration != duration )
                {
                    queryString.Append( $"&duration={duration}" );
                }

                IRestResponse response = await GetRestResponseAsync( "editMWindow", queryString.ToString() );

                return JsonConvert.DeserializeObject<MaintenanceWindowsResult>( response.Content );
            }
            else
            {
                return existingMaintenanceWindow;
            }
        }

        /// <summary>
        /// Maintenance windows can be deleted using this method.
        /// </summary>
        /// <param name="id">Required</param>
        /// <returns></returns>
        public async Task<MaintenanceWindowsResult> DeleteMaintenanceWindowAsync ( int id )
        {
            MaintenanceWindowsResult existingMaintenanceWindow = await GetMaintenanceWindowsAsync( id );

            if ( existingMaintenanceWindow.MaintenanceWindows?.Count > 0 )
            {
                StringBuilder queryString = new StringBuilder( $"api_key={_apiKey}&format=json" );

                queryString.Append( $"&id={id}" );

                IRestResponse response = await GetRestResponseAsync( "deleteMWindow", queryString.ToString() );

                return JsonConvert.DeserializeObject<MaintenanceWindowsResult>( response.Content );
            }
            else
            {
                return existingMaintenanceWindow;
            }
        }

        #endregion

        #region Public Status Pages

        /// <summary>
        /// The list of public status pages can be called with this method.
        /// </summary>
        /// <returns></returns>
        public async Task<PublicStatusPageResult> GetPublicStatusPagesAsync ()
        {
            return await GetPublicStatusPagesAsync( new PublicStatusPageRequest() );
        }

        /// <summary>
        /// The list of public status pages can be called with this method.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<PublicStatusPageResult> GetPublicStatusPagesAsync ( int id )
        {
            PublicStatusPageRequest request = new PublicStatusPageRequest { PublicStatusPages = new List<int> { id } };

            return await GetPublicStatusPagesAsync( request );
        }

        /// <summary>
        /// The list of public status pages can be called with this method.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<PublicStatusPageResult> GetPublicStatusPagesAsync ( PublicStatusPageRequest request )
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
        public async Task<PublicStatusPageResult> CreatePublicStatusPageAsync ( string friendlyName, List<int> monitors )
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
        public async Task<PublicStatusPageResult> CreatePublicStatusPageAsync ( string friendlyName, List<int> monitors, string customDomain, string password, PublicStatusPageSort sort )
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
                return PublicStatusPageError( ErrorType.NoFriendlyName );
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
        public async Task<PublicStatusPageResult> UpdatePublicStatusPageAsync ( int id, string friendlyName, List<int> monitors, string customDomain, string password, PublicStatusPageSort sort )
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
                return PublicStatusPageError( ErrorType.PublicStatusPage_NoPageFound );
            }
        }

        /// <summary>
        /// Public status pages can be deleted using this method.
        /// </summary>
        /// <param name="publicStatusPageId">Required</param>
        /// <returns></returns>
        public async Task<PublicStatusPageResult> DeletePublicStatusPageAsync ( int publicStatusPageId )
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
                return PublicStatusPageError( ErrorType.PublicStatusPage_NoPageFound );
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
        private async Task<IRestResponse> GetRestResponseAsync ( string endpoint, string query )
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
        private double ConvertDateTimeToSeconds ( DateTime date )
        {
            TimeSpan span = date.Subtract( new DateTime( 1970, 1, 1, 0, 0, 0, DateTimeKind.Utc ) );
            return span.TotalSeconds;
        }

        /// <summary>
        /// I got tired of using the same logic, especially since I didn't always know or get it right.
        /// There is a difference between IsNullOrEmpty and IsNullOrWhiteSpace.
        /// This makes it easier to change everything if I need to. 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private bool CheckString ( string value )
        {
            return string.IsNullOrWhiteSpace( value );
        }

        /// <summary>
        /// Maintenance Window Canned Errors 
        /// </summary>
        /// <param name="errorType"></param>
        /// <returns></returns>
        private MaintenanceWindowsResult MaintenanceWindowsError ( ErrorType errorType )
        {
            if ( errorType == ErrorType.NoFriendlyName )
            {
                return new MaintenanceWindowsResult
                {
                    Status = Status.fail,
                    Error = new Error
                    {
                        Explanation = _defaultExplanation,
                        Message = "No Friendly Name Was Provided",
                        ErrorType = errorType
                    }
                };
            }

            if ( errorType == ErrorType.MaintenanceWindow_WindowTypeRequiresValue )
            {
                return new MaintenanceWindowsResult
                {
                    Status = Status.fail,
                    Error = new Error
                    {
                        Explanation = _defaultExplanation,
                        Message = "A Value must be provided when using the Weekly or Monthly monitor type.",
                        ErrorType = errorType
                    }
                };
            }

            return null;
        }

        /// <summary>
        /// Public Status Page Canned Errors 
        /// </summary>
        /// <param name="errorType"></param>
        /// <returns></returns>
        private PublicStatusPageResult PublicStatusPageError ( ErrorType errorType )
        {
            if ( errorType == ErrorType.NoFriendlyName )
            {
                return new PublicStatusPageResult
                {
                    Status = Status.fail,
                    Error = new Error
                    {
                        Explanation = _defaultExplanation,
                        Message = "No Friendly Name Was Provided",
                        ErrorType = errorType
                    }
                };
            }

            if ( errorType == ErrorType.PublicStatusPage_NoPageFound )
            {
                return new PublicStatusPageResult
                {
                    Status = Status.fail,
                    Error = new Error
                    {
                        Explanation = _defaultExplanation,
                        Message = "No Public Status Page Was Found",
                        ErrorType = errorType
                    }
                };
            }

            return null;
        }

        #endregion
    }
}
