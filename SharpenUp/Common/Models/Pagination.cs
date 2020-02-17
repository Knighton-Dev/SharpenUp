using Newtonsoft.Json;

namespace SharpenUp.Common.Models
{
    public class Pagination
    {
        [JsonProperty( PropertyName = "offset" )]
        public int Offset { get; set; }

        [JsonProperty( PropertyName = "limit" )]
        public int Limit { get; set; }

        [JsonProperty( PropertyName = "total" )]
        public int Total { get; set; }
    }
}
