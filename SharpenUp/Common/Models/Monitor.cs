using Newtonsoft.Json;
using SharpenUp.Common.Types;
using System.Collections.Generic;
using System;

namespace SharpenUp.Common.Models
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

        [JsonProperty( PropertyName = "alert_contacts" )]
        public List<AlertContact> AlertContacts { get; set; }

        [JsonProperty( PropertyName = "ssl" )]
        public SSLInfo SSLInfo { get; set; }
    }
}
