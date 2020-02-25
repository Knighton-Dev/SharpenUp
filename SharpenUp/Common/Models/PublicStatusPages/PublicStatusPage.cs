using Newtonsoft.Json;
using System.Collections.Generic;
using SharpenUp.Common.Types;
using System.Diagnostics.CodeAnalysis;

namespace SharpenUp.Common.Models.PublicStatusPages
{
    public class PublicStatusPage
    {
        [ExcludeFromCodeCoverage]
        [JsonProperty( PropertyName = "id" )]
        public int Id { get; set; }

        [JsonProperty( PropertyName = "friendly_name" )]
        public string Name { get; set; }

        // TODO: This isn't parsing and I need to figure out why.
        //[JsonProperty( PropertyName = "monitors" )]
        //public List<int> Monitors { get; set; }

        [JsonProperty( PropertyName = "sort" )]
        public PublicStatusPageSortType Sort { get; set; }

        [JsonProperty( PropertyName = "status" )]
        public PublicStatusPageStatusType Status { get; set; }

        [JsonProperty( PropertyName = "standard_url" )]
        public string StandardURL { get; set; }

        [JsonProperty( PropertyName = "custom_url" )]
        public string CustomURL { get; set; }
    }
}
