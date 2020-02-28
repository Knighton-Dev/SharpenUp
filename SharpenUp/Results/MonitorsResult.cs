using System.Collections.Generic;
using Newtonsoft.Json;
using SharpenUp.Models;

namespace SharpenUp.Results
{
    public class MonitorsResult
    {
        [JsonProperty( PropertyName = "stat" )]
        public Status Status { get; set; }

        [JsonProperty( PropertyName = "pagination" )]
        public Pagination Pagination { get; set; }

        [JsonProperty( PropertyName = "monitors" )]
        public List<Monitor> Monitors { get; set; }

        [JsonProperty( PropertyName = "error" )]
        public Error Error { get; set; }
    }
}
