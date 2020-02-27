using System.Collections.Generic;
using Newtonsoft.Json;
using SharpenUp.Models;

namespace SharpenUp.Results
{
    public class PublicStatusPageResult
    {
        [JsonProperty( PropertyName = "stat" )]
        public Status Status { get; set; }

        [JsonProperty( PropertyName = "limit" )]
        public int Limit { get; set; }

        [JsonProperty( PropertyName = "offset" )]
        public int Offset { get; set; }

        [JsonProperty( PropertyName = "total" )]
        public int Total { get; set; }

        [JsonProperty( PropertyName = "psps" )]
        public List<PublicStatusPage> PublicStatusPages { get; set; }

        [JsonProperty( PropertyName = "error" )]
        public Error Error { get; set; }
    }
}
