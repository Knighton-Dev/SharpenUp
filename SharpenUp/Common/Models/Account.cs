using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;

namespace SharpenUp.Common.Models
{
    public class Account
    {
        [JsonProperty( PropertyName = "email" )]
        public string Email { get; set; }

        [JsonProperty( PropertyName = "monitor_limit" )]
        public int MonitorLimit { get; set; }

        [JsonProperty( PropertyName = "monitor_interval" )]
        public int MonitorInterval { get; set; }

        [JsonProperty( PropertyName = "up_monitors" )]
        [ExcludeFromCodeCoverage]
        public int UpMonitors { get; set; }

        [ExcludeFromCodeCoverage]
        [JsonProperty( PropertyName = "down_monitors" )]
        public int DownMonitors { get; set; }

        [ExcludeFromCodeCoverage]
        [JsonProperty( PropertyName = "paused_monitors" )]
        public int PausedMonitors { get; set; }
    }
}
