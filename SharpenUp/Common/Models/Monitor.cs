using Newtonsoft.Json;
using SharpenUp.Common.Types;
using System.Collections.Generic;

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
        public int CreationTime { get; set; } // TODO: Need to convert this to DateTime

        [JsonProperty( PropertyName = "status" )]
        public OnlineStatusType OnlineStatus { get; set; }

        [JsonProperty( PropertyName = "logs" )]
        public List<Log> Logs { get; set; }
    }
}
