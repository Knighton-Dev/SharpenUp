using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace SharpenUp.Common.Models.MaintenanceWindows
{
    [ExcludeFromCodeCoverage]
    public class MaintenanceWindowsRequest
    {
        public List<int> MaintenanceWindows { get; set; }
        public int PaginationOffset { get; set; }
        public int PaginationLimit { get; set; }
    }
}
