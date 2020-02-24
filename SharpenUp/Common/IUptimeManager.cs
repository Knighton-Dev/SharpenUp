using System.Threading.Tasks;
using SharpenUp.Common.Models.Accounts;
using SharpenUp.Common.Models.Alerts;
using SharpenUp.Common.Models.Monitors;
using SharpenUp.Common.Models.PublicStatusPages;

namespace SharpenUp.Common
{
    public interface IUptimeManager
    {
        Task<AccountDetailsResult> GetAccountDetailsAsync();
        Task<AlertContactsResult> GetAlertContactsAsync();
        Task<MonitorsResult> GetMonitorsAsync();
        Task<MonitorsResult> GetMonitorsAsync( MonitorsRequest request );
        Task<PublicStatusPagesResult> GetPublicStatusPagesAsync();
        Task<PublicStatusPagesResult> GetPublicStatusPagesAsync( PublicStatusPagesRequest request );
    }
}
