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
        private int? ExpiresInt { get; set; }

        public DateTime? Expires
        {
            get
            {
                if ( ExpiresInt.HasValue )
                {
                    DateTimeOffset offset = DateTimeOffset.FromUnixTimeSeconds( ExpiresInt.Value );
                    return offset.UtcDateTime;
                }
                return null;
            }
        }

        [JsonProperty( PropertyName = "ignore_errors" )]
        public bool? IgnoreErrors { get; set; }

        [JsonProperty( PropertyName = "disable_notifications" )]
        public bool? DisableNotifications { get; set; }
    }
}
