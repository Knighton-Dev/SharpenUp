using System.Threading.Tasks;
using SharpenUp.Common.Models;
using System.Collections.Generic;

namespace SharpenUp.Common
{
    public interface IUptimeManager
    {
        Task<AccountDetailsResult> GetAccountDetailsAsync();
        Task<MonitorsResult> GetMonitorAsync( int monitorId );
        Task<MonitorsResult> GetMonitorsAsync();
        Task<MonitorsResult> GetMonitorsAsync( List<int> monitorIds );
    }
}
