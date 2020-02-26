using Newtonsoft.Json;
using System;
using SharpenUp.Common.Types;

namespace SharpenUp.Common.Models
{
    public class Log
    {
        [JsonProperty( PropertyName = "type" )]
        public OnlineStatusType OnlineStatus { get; set; }

        [JsonProperty( PropertyName = "datetime" )]
        private int IncidentTimeInt { get; set; }

        public DateTime IncidentTime
        {
            get
            {
                DateTimeOffset offset = DateTimeOffset.FromUnixTimeSeconds( IncidentTimeInt );
                return offset.UtcDateTime;
            }
        }

        [JsonProperty( PropertyName = "duration" )]
        public int Duration { get; set; } // TODO: Need to convert this to TimeSpan

        [JsonProperty( PropertyName = "reason" )]
        public Reason Reason { get; set; }
    }
}
