using System.Collections.Generic;
using Newtonsoft.Json;
using SharpenUp.Models;

namespace SharpenUp.Results
{
    public class PublicStatusPageResult
    {
        [JsonProperty( PropertyName = "stat" )]
        public Status Status { get; set; }

        [JsonProperty( PropertyName = "pagination" )]
        public Pagination Pagination { get; set; }

        [JsonProperty( PropertyName = "psps" )]
        public List<PublicStatusPage> PublicStatusPages { get; set; }

        [JsonProperty( PropertyName = "error" )]
        public Error Error { get; set; }
    }
}
