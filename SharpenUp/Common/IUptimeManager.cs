using System.Threading.Tasks;
using SharpenUp.Common.Models;

namespace SharpenUp.Common
{
    public interface IUptimeManager
    {
        Task<AccountDetailsResult> GetAccountDetailsAsync();
        Task<MonitorsResult> GetMonitorsAsync( MonitorsRequest request );
    }
}
