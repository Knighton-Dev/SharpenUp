using Newtonsoft.Json;

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
        public int UpMonitors { get; set; }

        [JsonProperty( PropertyName = "down_monitors" )]
        public int DownMonitors { get; set; }

        [JsonProperty( PropertyName = "paused_monitors" )]
        public int PausedMonitors { get; set; }
    }
}
