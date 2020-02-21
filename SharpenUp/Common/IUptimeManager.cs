using System.Threading.Tasks;
using SharpenUp.Common.Models.Accounts;
using SharpenUp.Common.Models.Alerts;
using SharpenUp.Common.Models.Monitors;

namespace SharpenUp.Common
{
    public interface IUptimeManager
    {
        Task<AccountDetailsResult> GetAccountDetailsAsync();
        Task<AlertContactsResult> GetAlertContactsAsync();
        Task<MonitorsResult> GetMonitorsAsync();
        Task<MonitorsResult> GetMonitorsAsync( MonitorsRequest request );
    }
}
