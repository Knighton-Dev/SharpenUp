using Newtonsoft.Json;
using SharpenUp.Common.Types;

namespace SharpenUp.Common.Models
{
    public class Log
    {
        [JsonProperty( PropertyName = "type" )]
        public OnlineStatusType OnlineStatus { get; set; }

        [JsonProperty( PropertyName = "datetime" )]
        public int IncidentTime { get; set; } // TODO: Need to convert this from epoch to DateTime

        [JsonProperty( PropertyName = "duration" )]
        public int Duration { get; set; } // TODO: Need to convert this to TimeSpan

        [JsonProperty( PropertyName = "reason" )]
        public Reason Reason { get; set; }
    }
}
