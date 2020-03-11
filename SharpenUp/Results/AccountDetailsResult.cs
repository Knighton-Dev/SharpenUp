using Newtonsoft.Json;
using SharpenUp.Models;

namespace SharpenUp.Results
{
    public class AccountDetailsResult : BaseResult
    {
        [JsonProperty( PropertyName = "account" )]
        public Account Account { get; set; }
    }
}
