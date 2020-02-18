using Newtonsoft.Json;

namespace SharpenUp.Common.Models
{
    public class AlertContact
    {
        [JsonProperty(PropertyName="id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName="value")]
        public string Value { get; set; }

        [JsonProperty(PropertyName="type")]
        public int Type { get; set; } // TODO: Map out this type

        [JsonProperty(PropertyName="threshold")]
        public int Threshold { get; set; }

        [JsonProperty(PropertyName="recurrence")]
        public int Recurrence { get; set; }
    }
}
