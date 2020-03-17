using System.Collections.Generic;
using Newtonsoft.Json;
using SharpenUp.Models;

namespace SharpenUp.Results
{
    public class PublicStatusPageResult : BaseResult
    {
        [JsonProperty( PropertyName = "psps" )]
        public List<PublicStatusPage> PublicStatusPages { get; set; }

        [JsonProperty( PropertyName = "psp" )]
        public BasePublicStatusPage BasePublicStatusPage { get; set; }
    }
}
