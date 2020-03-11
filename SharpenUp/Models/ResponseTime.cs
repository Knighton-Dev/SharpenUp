using System;
using Newtonsoft.Json;

namespace SharpenUp.Models
{
    public class ResponseTime
    {
        /// <summary>
        /// The date and time of the log (inherits the user's timezone setting).
        /// </summary>
        [JsonProperty( PropertyName = "datetime" )]
        private int DateTimeInteger { get; set; }

        public DateTime DateTime
        {
            get
            {
                DateTimeOffset offset = DateTimeOffset.FromUnixTimeSeconds( DateTimeInteger );
                return offset.UtcDateTime;
            }
        }

        /// <summary>
        /// The time to first-byte in milliseconds.
        /// </summary>
        [JsonProperty( PropertyName = "value" )]
        public int Value { get; set; }
    }
}
