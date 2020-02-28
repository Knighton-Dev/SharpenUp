using System;
using System.Collections.Generic;
using SharpenUp.Models;
using System.Diagnostics.CodeAnalysis;

namespace SharpenUp.Requests
{
    public class MonitorsRequest
    {
        /// <summary>
        /// Optional (if not used, will return all monitors in an account. Else, it is possible to define any number of monitors with their IDs like: monitors=15830-32696-83920).
        /// </summary>
        public List<int> Monitors { get; set; } = null;

        /// <summary>
        /// Optional (if not used, will return all monitors types (HTTP, keyword, ping..) in an account. Else, it is possible to define any number of monitor types like: types=1-3-4).
        /// </summary>
        public List<MonitorType> MonitorTypes { get; set; } = null;
    }
}
