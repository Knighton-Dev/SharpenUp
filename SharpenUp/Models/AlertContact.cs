using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace SharpenUp.Models
{
    [ExcludeFromCodeCoverage]
    public class AlertContact
    {
        /// <summary>
        /// The ID of the alert contact.
        /// </summary>
        [JsonProperty( PropertyName = "id" )]
        public int Id { get; set; }

        /// <summary>
        /// The type of the alert contact notified (Zapier, HipChat and Slack are not supported in the newAlertContact method yet).
        /// </summary>
        [JsonProperty( PropertyName = "type" )]
        public ContactType ContactType { get; set; }

        /// <summary>
        /// Friendly name of the alert contact (for making it easier to distinguish from others).
        /// </summary>
        [JsonProperty( PropertyName = "friendly_name" )]
        public string FriendlyName { get; set; }

        /// <summary>
        /// Alert contact's address/phone.
        /// </summary>
        [JsonProperty( PropertyName = "value" )]
        public string Value { get; set; }

        /// <summary>
        /// The status of the alert contact.
        /// </summary>
        [JsonProperty( PropertyName = "status" )]
        public ContactStatus ContactStatus { get; set; }

        /// <summary>
        /// The x value that is set to define "if down for x minutes, alert every y minutes.
        /// </summary>
        [JsonProperty( PropertyName = "threshold" )]
        public int Threshold { get; set; }

        /// <summary>
        /// The y value that is set to define "if down for x minutes, alert every y minutes.
        /// </summary>
        [JsonProperty( PropertyName = "recurrence" )]
        public int Recurrence { get; set; }
    }

    public enum ContactType
    {
        SMS = 1,
        Email = 2,
        Twitter = 3,
        Boxcar = 4,
        WebHook = 5,
        PushBullet = 6,
        Zapier = 7,
        Pushover = 9,
        HipChat = 10,
        Slack = 11
    }

    public enum ContactStatus
    {
        NotActivated = 0,
        Paused = 1,
        Activated = 2
    }
}
