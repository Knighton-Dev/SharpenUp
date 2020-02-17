using System;
using System.Threading.Tasks;
using SharpenUp.Common.Models;

namespace SharpenUp.Common
{
    public interface IUptimeManager
    {
        Task<AccountDetails> GetAccountDetailsAsync();
    }
}
