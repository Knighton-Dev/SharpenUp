using Newtonsoft.Json;
using SharpenUp.Common.Types;

namespace SharpenUp.Common.Models.PublicStatusPages
{
    public class PublicStatusPage
    {
        [JsonProperty( PropertyName = "id" )]
        public int Id { get; set; }

        [JsonProperty( PropertyName = "friendly_name" )]
        public string Name { get; set; }

        [JsonProperty( PropertyName = "monitors" )]
        public int Monitors { get; set; }

        [JsonProperty( PropertyName = "sort" )]
        public int Sort { get; set; }

        [JsonProperty( PropertyName = "status" )]
        public PublicStatusPageStatusType Status { get; set; }

        [JsonProperty( PropertyName = "standard_url" )]
        public string StandardURL { get; set; }

        [JsonProperty( PropertyName = "custom_url" )]
        public string CustomURL { get; set; }
    }
}
