using System;
using System.Collections.Generic;
using SharpenUp.Common.Types;

namespace SharpenUp.Common.Models
{
    public class MonitorsRequest
    {
        public List<int> MonitorIds { get; set; } = new List<int>();
        public List<MonitorType> MonitorTypes { get; set; } = new List<MonitorType>();
        public List<OnlineStatusType> StatusTypes { get; set; } = new List<OnlineStatusType>();
        public List<Tuple<DateTime, DateTime>> UptimeDateRanges { get; set; } = new List<Tuple<DateTime, DateTime>> { new Tuple<DateTime, DateTime>( DateTime.MinValue, DateTime.MaxValue ) };
        public bool IncludeAllTimeUptimeRatio { get; set; } = false;
        public bool IncludeAllTimeUptimeDurations { get; set; } = false;
        public bool IncludeLogs { get; set; } = false;
        public DateTime LogsStartDate { get; set; } = DateTime.MinValue;
        public DateTime LogsEndDate { get; set; } = DateTime.MaxValue;
        public int LogsLimit { get; set; } = 50;
        public bool IncludeResponseTimes { get; set; } = false;
        public DateTime ResponseTimesStartDate { get; set; } = DateTime.MinValue;
        public DateTime ResponseTimesEndDate { get; set; } = DateTime.MaxValue;
        public bool IncludeAlertContacts { get; set; } = false;
        public bool IncludeMaintenanceWindows { get; set; } = false;
        public bool IncludeCustomHttpHeaders { get; set; } = false;
        public bool IncludeCustomHttpStatus { get; set; } = false;
        public bool IncludeTimezone { get; set; } = false;
        public int Offset { get; set; } = 0;
        public int Limit { get; set; } = 50;
        public string SearchTerm { get; set; }
        public bool IncludeSSLInfo { get; set; } = false;
    }
}
