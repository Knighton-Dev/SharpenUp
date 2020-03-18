using System;
using System.Collections.Generic;
using SharpenUp.Models;

namespace SharpenUp.Requests
{
    public class MonitorsRequest
    {
        /// <summary>
        /// Optional (if not used, will return all monitors in an account. Else, it is possible to define any number of monitors with their IDs like: monitors=15830-32696-83920).
        /// </summary>
        public List<int> Monitors { get; set; } = null;

        /// <summary>
        /// Optional (if not used, will return all monitors types (HTTP, keyword, ping..) in an account. Else, it is possible to define any number of monitor types like: types=1-3-4).
        /// </summary>
        public List<MonitorType> MonitorTypes { get; set; } = null;

        /// <summary>
        /// Optional (if not used, will return all monitors statuses (up, down, paused) in an account. Else, it is possible to define any number of monitor statuses like: statuses=2-9).
        /// </summary>
        public List<MonitorStatus> Statuses { get; set; } = null;

        /// <summary>
        /// Optional (defines the number of days to calculate the uptime ratio(s) for. Ex: custom_uptime_ratios=7-30-45 to get the uptime ratios for those periods).
        /// </summary>
        public List<int> CustomUptimeRatios { get; set; } = null;

        /// <summary>
        /// Optional (defines the ranges to calculate the uptime ratio(s) for. Ex: custom_uptime_ranges=1465440758_1466304758 to get the uptime ratios for those periods. It is possible to send multiple ranges like 1465440758_1466304758-1434682358_1434855158).
        /// </summary>
        public List<Tuple<DateTime, DateTime>> CustomUptimeRanges { get; set; } = null;

        /// <summary>
        /// Optional (returns the "all time uptime ratio". It will slow down the response a bit and, if not really necessary, suggest not using it. Default is 0).
        /// </summary>
        public bool AllTimeUptimeRatio { get; set; } = false;

        /// <summary>
        /// Optional (returns the "all time durations of up-down-paused events". It will slow down the response a bit and, if not really necessary, suggest not using it. Default is 0).
        /// </summary>
        public bool AllTimeUptimeDurations { get; set; } = false;

        /// <summary>
        /// Optional (defines if the logs of each monitor will be returned. Should be set to 1 for getting the logs. Default is 0).
        /// </summary>
        public bool IncludeLogs { get; set; } = false;

        /// <summary>
        /// Optional (works only for the Pro Plan as 24 hour+ logs are kept only in the Pro Plan, formatted as Unix time and must be used with logs_end_date).
        /// </summary>
        public DateTime? LogsStartDate { get; set; } = null;

        /// <summary>
        /// Optional (works only for the Pro Plan as 24 hour+ logs are kept only in the Pro Plan, formatted as Unix time and must be used with logs_start_date).
        /// </summary>
        public DateTime? LogsEndDate { get; set; } = null;

        /// <summary>
        /// Optional (the types of logs to be returned with a usage like: log_types=1-2-98). If empty, all log types are returned.
        /// </summary>
        public List<LogType> LogTypes { get; set; } = null;

        /// <summary>
        /// Optional (the number of logs to be returned in descending order). If empty, all logs are returned.
        /// </summary>
        public int? LogsLimit { get; set; } = null;

        /// <summary>
        /// Optional (defines if the response time data of each monitor will be returned. Should be set to 1 for getting them. Default is 0).
        /// </summary>
        public bool ResponseTimes { get; set; } = false;

        /// <summary>
        /// Optional (the number of response time logs to be returned (descending order). If empty, last 24 hours of logs are returned (if response_times_start_date and response_times_end_date are not used).
        /// </summary>
        public int? ResponseTimesLimit { get; set; } = null;

        /// <summary>
        /// Optional (by default, response time value of each check is returned. The API can return average values in given minutes. Default is 0. For ex: the Uptime Robot dashboard displays the data averaged/grouped in 30 minutes).
        /// </summary>
        public int? ResponseTimesAverage { get; set; } = null;

        /// <summary>
        /// Optional (formatted as Unix time and must be used with response_times_end_date) (response_times_end_date - response_times_start_date can't be more than 7 days)
        /// </summary>
        public DateTime? ResponseTimesStartDate { get; set; } = null;

        /// <summary>
        /// Optional (formatted as Unix time and must be used with response_times_start_date) (response_times_end_date - response_times_start_date can't be more than 7 days)
        /// </summary>
        public DateTime? ResponseTimesEndDate { get; set; } = null;

        /// <summary>
        /// Optional (defines if the alert contacts set for the monitor to be returned. Default is 0.)
        /// </summary>
        public bool AlertContacts { get; set; } = false;

        /// <summary>
        /// Optional (defines if the maintenance windows for the monitors will be returned. Default is 0.)
        /// </summary>
        public bool MaintenanceWindows { get; set; } = false;

        /// <summary>
        /// Optional (defines if SSL certificate info for each monitor will be returned).
        /// </summary>
        public bool IncludeSSL { get; set; } = false;

        /// <summary>
        /// Optional (defines if the user's timezone should be returned. Should be set to 1 for getting it. Default is 0).
        /// </summary>
        public bool Timezone { get; set; } = false;

        /// <summary>
        /// Optional (used for pagination. Defines the record to start paginating. Default is 0)
        /// </summary>
        public int Offset { get; set; } = 0;

        /// <summary>
        /// Optional (used for pagination. Defines the max number of records to return for the response. Default and max. is 50)
        /// </summary>
        public int Limit { get; set; } = 50;

        public string Search { get; set; } = string.Empty;

        /// <summary>
        /// Optional (defines if the custom HTTP headers of each monitor will be returned. Should be set to 1 for getting them. Default is 0).
        /// NOT IMPLEMENTED
        /// </summary>
        public bool CustomHttpHeaders { get; set; } = false;

        /// <summary>
        /// Optional (defines if the custom HTTP statuses of each monitor will be returned. Should be set to 1 for getting them. Default is 0).
        /// NOT IMPLEMENTED
        /// </summary>
        public bool CustomHttpStatuses { get; set; } = false;
    }
}
