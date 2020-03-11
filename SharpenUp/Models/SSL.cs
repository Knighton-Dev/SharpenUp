using System;
using Newtonsoft.Json;

namespace SharpenUp.Models
{
    public class SSL
    {
        [JsonProperty( PropertyName = "brand" )]
        public string Brand { get; set; }

        [JsonProperty( PropertyName = "product" )]
        public string Product { get; set; }

        [JsonProperty( PropertyName = "expires" )]
        private int ExpiresInt { get; set; }

        public DateTime Expires
        {
            get
            {
                DateTimeOffset offset = DateTimeOffset.FromUnixTimeSeconds( ExpiresInt );
                return offset.UtcDateTime;
            }
        }

        [JsonProperty( PropertyName = "ignore_errors" )]
        public bool IgnoreErrors { get; set; }

        [JsonProperty( PropertyName = "disable_notifications" )]
        public bool DisableNotifications { get; set; }
    }
}
