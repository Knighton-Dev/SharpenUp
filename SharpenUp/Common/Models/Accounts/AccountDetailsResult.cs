using Newtonsoft.Json;
using SharpenUp.Common.Types;

namespace SharpenUp.Common.Models.Accounts
{
    public class AccountDetailsResult
    {
        [JsonProperty( PropertyName = "stat" )]
        public RequestStatusType Status { get; set; }

        [JsonProperty( PropertyName = "error" )]
        public Error Error { get; set; }

        [JsonProperty( PropertyName = "account" )]
        public Account Account { get; set; }
    }
}
