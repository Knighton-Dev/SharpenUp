using System.Collections.Generic;
using Newtonsoft.Json;
using SharpenUp.Models;

namespace SharpenUp.Results
{
    public class MaintenanceWindowsResult
    {
        [JsonProperty( PropertyName = "stat" )]
        public Status Status { get; set; }

        [JsonProperty( PropertyName = "pagination" )]
        public Pagination Pagination { get; set; }

        [JsonProperty( PropertyName = "mwindows" )]
        public List<MaintenanceWindow> MaintenanceWindows { get; set; }

        [JsonProperty( PropertyName = "mwindow" )]
        public BaseMaintenanceWindow BaseMaintenanceWindow { get; set; }

        [JsonProperty( PropertyName = "error" )]
        public Error Error { get; set; }
    }
}
