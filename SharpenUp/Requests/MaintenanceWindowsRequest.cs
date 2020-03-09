using System.Collections.Generic;

namespace SharpenUp.Requests
{
    public class MaintenanceWindowsRequest
    {
        public List<int> MaintenanceWindows { get; set; }
        public int Offset { get; set; }
        public int Limit { get; set; }
    }
}
