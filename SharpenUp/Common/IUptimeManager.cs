using System.Threading.Tasks;
using SharpenUp.Common.Models;

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
