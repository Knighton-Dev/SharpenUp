using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;

namespace SharpenUp.Common.Models.Alerts
{
    public class AlertContact
    {
        [ExcludeFromCodeCoverage]
        [JsonProperty( PropertyName = "id" )]
        public int Id { get; set; }

        [JsonProperty( PropertyName = "value" )]
        public string Value { get; set; }

        [JsonProperty( PropertyName = "type" )]
        public int Type { get; set; } // TODO: Map out this type

        [ExcludeFromCodeCoverage]
        [JsonProperty( PropertyName = "threshold" )]
        public int Threshold { get; set; }

        [ExcludeFromCodeCoverage]
        [JsonProperty( PropertyName = "recurrence" )]
        public int Recurrence { get; set; }
    }
}
