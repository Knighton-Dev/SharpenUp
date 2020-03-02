using System;
using System.Collections.Generic;
using SharpenUp.Models;
using System.Diagnostics.CodeAnalysis;

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
        [ExcludeFromCodeCoverage]
        public List<MonitorStatus> Statuses { get; set; } = null;

        /// <summary>
        /// Optional (defines the number of days to calculate the uptime ratio(s) for. Ex: custom_uptime_ratios=7-30-45 to get the uptime ratios for those periods).
        /// </summary>
        [ExcludeFromCodeCoverage]
        public List<int> CustomUptimeRatios { get; set; } = null;

        // TODO: Custom Uptime Ranges

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

        // TODO: Logs Start Date
        // TODO: Logs End Date

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

        // TODO: Response Times Average
        // TODO: Response Times Start Date
        // TODO: Response Times End Date

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
        /// </summary>
        //public bool CustomHttpHeaders { get; set; } = false;

        /// <summary>
        /// Optional (defines if the custom HTTP statuses of each monitor will be returned. Should be set to 1 for getting them. Default is 0).
        /// </summary>
        //public bool CustomHttpStatuses { get; set; } = false;
    }
}
