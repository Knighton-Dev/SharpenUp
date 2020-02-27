using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;
using SharpenUp.Common.Types;

namespace SharpenUp.Common.Models.MaintenanceWindows
{
    public class MaintenanceWindow
    {
        [ExcludeFromCodeCoverage]
        [JsonProperty( PropertyName = "id" )]
        public int Id { get; set; }

        [JsonProperty( PropertyName = "user" )]
        public int User { get; set; } // TODO: This might be a type.

        [JsonProperty( PropertyName = "type" )]
        public MaintenanceWindowType WindowType { get; set; }

        [JsonProperty( PropertyName = "friendly_name" )]
        public string FriendlyName { get; set; }

        [JsonProperty( PropertyName = "duration" )]
        public int Duration { get; set; }

        [JsonProperty( PropertyName = "status" )]
        public MaintenanceWindowStatusType Status { get; set; }

        [JsonProperty( PropertyName = "value" )]
        public string Value { get; set; }

        [JsonProperty( PropertyName = "start_time" )]
        public string StartTime { get; set; } // TODO: I don't know what this type is.
    }
}
