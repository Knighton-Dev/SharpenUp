using System.Collections.Generic;
using Newtonsoft.Json;
using SharpenUp.Common.Types;

namespace SharpenUp.Common.Models.MaintenanceWindows
{
    public class MaintenanceWindowsResult
    {
        [JsonProperty( PropertyName = "stat" )]
        public RequestStatusType Status { get; set; }

        [JsonProperty( PropertyName = "pagination" )]
        public Pagination Pagination { get; set; }

        [JsonProperty( PropertyName = "error" )]
        public Error Error { get; set; }

        [JsonProperty( PropertyName = "mwindows" )]
        public List<MaintenanceWindow> Results { get; set; }
    }
}
