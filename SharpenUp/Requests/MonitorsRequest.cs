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
    }
}
