using System;
using Newtonsoft.Json;

namespace SharpenUp.Models
{
    public class PublicStatusPage
    {
        /// <summary>
        /// The ID of the status page.
        /// </summary>
        [JsonProperty( PropertyName = "id" )]
        public int Id { get; set; }

        /// <summary>
        /// Friendly name of the status page (for making it easier to distinguish from others).
        /// </summary>
        [JsonProperty( PropertyName = "friendly_name" )]
        public string FriendlyName { get; set; }

        /// <summary>
        /// The list of monitorIDs to be displayed in status page (the values are seperated with "-" or 0 for all monitors).
        /// </summary>
        [JsonProperty( PropertyName = "monitors" )]
        public string Monitors { get; set; }

        /// <summary>
        /// The domain or subdomain that the status page will run on.
        /// </summary>
        [JsonProperty( PropertyName = "custom_domain" )]
        public Uri CustomDomain { get; set; }

        /// <summary>
        /// The password for the status page.
        /// </summary>
        [JsonProperty( PropertyName = "password" )]
        public string Password { get; set; }

        /// <summary>
        /// The sorting of the status page.
        /// </summary>
        [JsonProperty( PropertyName = "sort" )]
        public PublicStatusPageSort PublicStatusPageSort { get; set; }

        /// <summary>
        /// The status of the status page.
        /// </summary>
        [JsonProperty( PropertyName = "status" )]
        public PublicStatusPageStatus PublicStatusPageStatus { get; set; }
    }

    public enum PublicStatusPageSort
    {
        FriendlyNameAscending = 1,
        FriendlyNameDescending = 2,
        Status = 3,
        StatusReverse = 4
    }

    public enum PublicStatusPageStatus
    {
        Paused = 0,
        Active = 1
    }
}
