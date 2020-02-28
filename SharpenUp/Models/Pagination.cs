using Newtonsoft.Json;

namespace SharpenUp.Models
{
    public class Pagination
    {
        /// <summary>
        /// The starting record for getMonitors and getAlertContacts methods.
        /// </summary>
        [JsonProperty( PropertyName = "offset" )]
        public int Offset { get; set; }

        /// <summary>
        /// The number of records to be returned for getMonitors and getAlertContacts methods.
        /// </summary>
        [JsonProperty( PropertyName = "limit" )]
        public int Limit { get; set; }

        /// <summary>
        /// The total number of records for getMonitors and getAlertContacts methods.
        /// </summary>
        [JsonProperty( PropertyName = "total" )]
        public int Total { get; set; }
    }
}
