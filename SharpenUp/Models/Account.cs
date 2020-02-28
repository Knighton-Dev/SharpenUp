using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;

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
        [ExcludeFromCodeCoverage]
        public int MonitorLimit { get; set; }

        /// <summary>
        /// The min monitoring interval (in seconds) supported by the account.
        /// </summary>
        [JsonProperty( PropertyName = "monitor_interval" )]
        [ExcludeFromCodeCoverage]
        public int MonitorInterval { get; set; }

        /// <summary>
        /// The number of "up" monitors.
        /// </summary>
        [JsonProperty( PropertyName = "up_monitors" )]
        [ExcludeFromCodeCoverage]
        public int UpMonitors { get; set; }

        /// <summary>
        /// The number of "down" monitors.
        /// </summary>
        [JsonProperty( PropertyName = "down_monitors" )]
        [ExcludeFromCodeCoverage]
        public int DownMonitors { get; set; }

        /// <summary>
        /// The number of "paused" monitors.
        /// </summary>
        [JsonProperty( PropertyName = "pause_monitors" )]
        [ExcludeFromCodeCoverage]
        public int PausedMonitors { get; set; }

    }
}
