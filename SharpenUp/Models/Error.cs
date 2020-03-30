using Newtonsoft.Json;

namespace SharpenUp.Models
{
    public class Error
    {
        [JsonProperty( PropertyName = "type" )]
        public string Explanation { get; set; }

        public ErrorType? ErrorType { get; set; }

        [JsonProperty( PropertyName = "parameter_name" )]
        public string ParameterName { get; set; }

        [JsonProperty( PropertyName = "passed_value" )]
        public string PassedValue { get; set; }

        [JsonProperty( PropertyName = "message" )]
        public string Message { get; set; }
    }

    public enum ErrorType
    {
        System,
        NoFriendlyName,
        MaintenanceWindow_WindowTypeRequiresValue,
        PublicStatusPage_NoPageFound
    }
}
