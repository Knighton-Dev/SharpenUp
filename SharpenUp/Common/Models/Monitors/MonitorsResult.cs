using System.Collections.Generic;
using Newtonsoft.Json;
using SharpenUp.Common.Types;

namespace SharpenUp.Common.Models.Monitors
{
    public class MonitorsResult
    {
        [JsonProperty( PropertyName = "stat" )]
        public RequestStatusType Status { get; set; }

        [JsonProperty( PropertyName = "pagination" )]
        public Pagination Pagination { get; set; }

        [JsonProperty( PropertyName = "error" )]
        public Error Error { get; set; }

        [JsonProperty( PropertyName = "monitors" )]
        public List<Monitor> Results { get; set; }

        [JsonProperty( PropertyName = "timezone" )]
        public int TimeZone { get; set; }
    }
}
