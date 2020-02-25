using System.Collections.Generic;
using Newtonsoft.Json;
using SharpenUp.Common.Types;

namespace SharpenUp.Common.Models.PublicStatusPages
{
    public class PublicStatusPagesResult
    {
        [JsonProperty( PropertyName = "stat" )]
        public RequestStatusType Status { get; set; }

        [JsonProperty( PropertyName = "pagination" )]
        public Pagination Pagination { get; set; }

        [JsonProperty( PropertyName = "error" )]
        public Error Error { get; set; }

        [JsonProperty( PropertyName = "psps" )]
        public List<PublicStatusPage> Results { get; set; }
    }
}
