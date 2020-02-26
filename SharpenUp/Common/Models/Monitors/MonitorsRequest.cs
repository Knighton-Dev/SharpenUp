using System;
using System.Collections.Generic;
using SharpenUp.Common.Types;
using System.Diagnostics.CodeAnalysis;

namespace SharpenUp.Common.Models.Monitors
{
    public class MonitorsRequest
    {
        /// <summary>
        /// Optional (if not used, will return all monitors in an account. Else, it is possible to define any number of monitors with their IDs like: monitors=15830-32696-83920)
        /// </summary>
        public List<int> MonitorIds { get; set; } = new List<int>();

        /// <summary>
        /// Optional (if not used, will return all monitors types (HTTP, keyword, ping..) in an account. Else, it is possible to define any number of monitor types like: types=1-3-4)
        /// </summary>
        public List<MonitorType> MonitorTypes { get; set; } = new List<MonitorType>();

        /// <summary>
        /// Optional (if not used, will return all monitors statuses (up, down, paused) in an account. Else, it is possible to define any number of monitor statuses like: statuses=2-9)
        /// </summary>
        public List<OnlineStatusType> StatusTypes { get; set; } = new List<OnlineStatusType>();

        // TODO: Missing Custom Uptime Ratios (not sure what that even means)

        /// <summary>
        /// Optional (defines the ranges to calculate the uptime ratio(s) for. Ex: custom_uptime_ranges=1465440758_1466304758 to get the uptime ratios for those periods. It is possible to send multiple ranges like 1465440758_1466304758-1434682358_1434855158)
        /// </summary>
        public List<Tuple<DateTime, DateTime>> UptimeDateRanges { get; set; } = new List<Tuple<DateTime, DateTime>> { new Tuple<DateTime, DateTime>( DateTime.MinValue, DateTime.MaxValue ) };

        /// <summary>
        /// Optional (returns the "all time uptime ratio". It will slow down the response a bit and, if not really necessary, suggest not using it. Default is 0)
        /// </summary>
        public bool IncludeAllTimeUptimeRatio { get; set; } = false;

        /// <summary>
        /// Optional (returns the "all time durations of up-down-paused events". It will slow down the response a bit and, if not really necessary, suggest not using it. Default is 0)
        /// </summary>
        public bool IncludeAllTimeUptimeDurations { get; set; } = false;

        /// <summary>
        /// Optional (defines if the logs of each monitor will be returned. Should be set to 1 for getting the logs. Default is 0)
        /// </summary>
        public bool IncludeLogs { get; set; } = false;

        /// <summary>
        /// Optional (works only for the Pro Plan as 24 hour+ logs are kept only in the Pro Plan, formatted as Unix time and must be used with logs_end_date)
        /// </summary>
        public DateTime LogsStartDate { get; set; } = DateTime.MinValue;

        /// <summary>
        /// Optional (works only for the Pro Plan as 24 hour+ logs are kept only in the Pro Plan, formatted as Unix time and must be used with logs_start_date)
        /// </summary>
        public DateTime LogsEndDate { get; set; } = DateTime.MaxValue;

        // TODO: Missing Log Types Filter

        /// <summary>
        /// Optional (the number of logs to be returned in descending order). If empty, all logs are returned.
        /// </summary>
        public int LogsLimit { get; set; } = 50;

        /// <summary>
        /// Optional (defines if the response time data of each monitor will be returned. Should be set to 1 for getting them. Default is 0)
        /// </summary>
        public bool IncludeResponseTimes { get; set; } = false;

        // TODO: Missing Reponse Times Limit
        // TODO: Missing Respons Times Averags

        /// <summary>
        /// Optional (formatted as Unix time and must be used with response_times_end_date) (response_times_end_date - response_times_start_date can't be more than 7 days)
        /// </summary>
        public DateTime ResponseTimesStartDate { get; set; } = DateTime.MinValue;

        /// <summary>
        /// Optional (formatted as Unix time and must be used with response_times_start_date) (response_times_end_date - response_times_start_date can't be more than 7 days)
        /// </summary>
        public DateTime ResponseTimesEndDate { get; set; } = DateTime.MaxValue;

        /// <summary>
        /// Optional (defines if the alert contacts set for the monitor to be returned. Default is 0.)
        /// </summary>
        public bool IncludeAlertContacts { get; set; } = false;

        // TODO: Come back and test this when I have a premium account. 
        /// <summary>
        /// Optional (defines if the maintenance windows for the monitors will be returned. Default is 0.)
        /// </summary>
        [ExcludeFromCodeCoverage]
        public bool IncludeMaintenanceWindows { get; set; } = false;

        /// <summary>
        /// Optional (defines if SSL certificate info for each monitor will be returned)
        /// </summary>
        public bool IncludeSSLInfo { get; set; } = false;

        /// <summary>
        /// Optional (defines if the custom HTTP headers of each monitor will be returned. Should be set to 1 for getting them. Default is 0)
        /// </summary>
        public bool IncludeCustomHttpHeaders { get; set; } = false;

        /// <summary>
        /// Optional (defines if the custom HTTP statuses of each monitor will be returned. Should be set to 1 for getting them. Default is 0)
        /// </summary>
        public bool IncludeCustomHttpStatus { get; set; } = false;

        /// <summary>
        /// Optional (defines if the user's timezone should be returned. Should be set to 1 for getting it. Default is 0)
        /// </summary>
        public bool IncludeTimezone { get; set; } = false;

        /// <summary>
        /// Optional (used for pagination. Defines the record to start paginating. Default is 0)
        /// </summary>
        public int PaginationOffset { get; set; } = 0;

        /// <summary>
        /// Optional (used for pagination. Defines the max number of records to return for the response. Default and max. is 50)
        /// </summary>
        public int PaginationLimit { get; set; } = 50;

        /// <summary>
        /// Optional (a keyword of your choice to search within url and friendly_name and get filtered results)
        /// </summary>
        public string SearchTerm { get; set; }
    }
}
