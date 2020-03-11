using System.Collections.Generic;
using Newtonsoft.Json;
using SharpenUp.Models;

namespace SharpenUp.Results
{
    public class MaintenanceWindowsResult : BaseResult
    {
        [JsonProperty( PropertyName = "mwindows" )]
        public List<MaintenanceWindow> MaintenanceWindows { get; set; }

        [JsonProperty( PropertyName = "mwindow" )]
        public BaseMaintenanceWindow BaseMaintenanceWindow { get; set; }
    }
}
