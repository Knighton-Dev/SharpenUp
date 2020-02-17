using Newtonsoft.Json;

namespace SharpenUp.Common.Models
{
    public class Error
    {
        [JsonProperty( PropertyName = "type" )]
        public string Type { get; set; }

        [JsonProperty( PropertyName = "parameter_name" )]
        public string ParameterName { get; set; }

        [JsonProperty( PropertyName = "passed_value" )]
        public string PassedValue { get; set; }

        [JsonProperty( PropertyName = "message" )]
        public string Message { get; set; }
    }
}
