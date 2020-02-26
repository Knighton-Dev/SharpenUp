using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using SharpenUp.Common.Models.Alerts;
using SharpenUp.Common.Types;

namespace SharpenUp.Common.Models.Monitors
{
    public class Monitor
    {
        [JsonProperty( PropertyName = "id" )]
        public int Id { get; set; }

        [JsonProperty( PropertyName = "friendly_name" )]
        public string FriendlyName { get; set; }

        [JsonProperty( PropertyName = "url" )]
        public string URL { get; set; }

        [JsonProperty( PropertyName = "type" )]
        public MonitorType MonitorType { get; set; }

        [JsonProperty( PropertyName = "interval" )]
        public int Interval { get; set; }

        [JsonProperty( PropertyName = "create_datetime" )]
        private int CreationTimeInt { get; set; }

        public DateTime CreationDate
        {
            get
            {
                DateTimeOffset offset = DateTimeOffset.FromUnixTimeSeconds( CreationTimeInt );
                return offset.UtcDateTime;
            }
        }

        [JsonProperty( PropertyName = "status" )]
        public OnlineStatusType OnlineStatus { get; set; }

        [JsonProperty( PropertyName = "logs" )]
        public List<Log> Logs { get; set; }

        [JsonProperty( PropertyName = "all_time_uptime_ratio" )]
        public double UptimeRatio { get; set; }

        [JsonProperty( PropertyName = "custom_uptime_ranges" )]
        public double CustomUptimeRatio { get; set; }

        [JsonProperty( PropertyName = "alert_contacts" )]
        public List<AlertContact> AlertContacts { get; set; }

        [JsonProperty( PropertyName = "ssl" )]
        public SSLInfo SSLInfo { get; set; }

        [JsonProperty( PropertyName = "all_time_uptime_durations" )]
        private string UptimeDurationString { get; set; }

        public UptimeDuration UptimeDuration
        {
            get
            {
                List<string> brokenUp = UptimeDurationString.Split( '-' ).ToList();

                return new UptimeDuration
                {
                    Up = int.Parse( brokenUp[ 0 ] ),
                    Down = int.Parse( brokenUp[ 1 ] ),
                    Paused = int.Parse( brokenUp[ 2 ] )
                };
            }
        }
    }
}
