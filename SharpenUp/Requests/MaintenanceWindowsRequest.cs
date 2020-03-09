using System.Collections.Generic;

namespace SharpenUp.Requests
{
    public class MaintenanceWindowsRequest
    {
        public List<int> MaintenanceWindows { get; set; } = null;
        public int Offset { get; set; } = 0;
        public int Limit { get; set; } = 50;
    }
}
