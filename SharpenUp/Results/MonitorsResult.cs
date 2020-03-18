using System.Collections.Generic;
using Newtonsoft.Json;
using SharpenUp.Models;

namespace SharpenUp.Results
{
    public class MonitorsResult : BaseResult
    {
        [JsonProperty( PropertyName = "monitors" )]
        public List<Monitor> Monitors { get; set; }

        [JsonProperty( PropertyName = "monitor" )]
        public BaseMonitor BaseMonitor { get; set; }

        [JsonProperty( PropertyName = "timezone" )]
        public int? Timezone { get; set; }
    }
}
