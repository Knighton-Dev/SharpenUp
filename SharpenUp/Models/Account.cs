using Newtonsoft.Json;

namespace SharpenUp.Models
{
    public class Account
    {
        /// <summary>
        /// The account e-mail.
        /// </summary>
        [JsonProperty( PropertyName = "email" )]
        public string Email { get; set; }

        /// <summary>
        /// The max number of monitors that can be created for the account.
        /// </summary>
        [JsonProperty( PropertyName = "monitor_limit" )]
        public int MonitorLimit { get; set; }

        /// <summary>
        /// The min monitoring interval (in seconds) supported by the account.
        /// </summary>
        [JsonProperty( PropertyName = "monitor_interval" )]
        public int MonitorInterval { get; set; }

        /// <summary>
        /// The number of "up" monitors.
        /// </summary>
        [JsonProperty( PropertyName = "up_monitors" )]
        public int UpMonitors { get; set; }

        /// <summary>
        /// The number of "down" monitors.
        /// </summary>
        [JsonProperty( PropertyName = "down_monitors" )]
        public int DownMonitors { get; set; }

        /// <summary>
        /// The number of "paused" monitors.
        /// </summary>
        [JsonProperty( PropertyName = "pause_monitors" )]
        public int PausedMonitors { get; set; }

    }
}
