using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace SharpenUp.Common.Models
{
    [ExcludeFromCodeCoverage]
    public class SSLInfo
    {
        [JsonProperty( PropertyName = "brand" )]
        public string Brand { get; set; }

        [JsonProperty( PropertyName = "product" )]
        public string Product { get; set; }

        [JsonProperty( PropertyName = "expires" )]
        public int Expires { get; set; } // TODO: Convert to DateTime

        [JsonProperty( PropertyName = "ignore_errors" )]
        public bool IgnoreErrors { get; set; } // TODO: May not map

        [JsonProperty( PropertyName = "disable_notifications" )]
        public bool DisbaleNotifications { get; set; }
    }
}
