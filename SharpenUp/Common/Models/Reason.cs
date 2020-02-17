using Newtonsoft.Json;

namespace SharpenUp.Common.Models
{
    public class Reason
    {
        [JsonProperty( PropertyName = "code" )]
        public string Code { get; set; }

        [JsonProperty( PropertyName = "detail" )]
        public string Detail { get; set; }
    }
}
