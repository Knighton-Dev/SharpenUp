using Newtonsoft.Json;
using SharpenUp.Models;

namespace SharpenUp.Results
{
    public class AccountDetailsResult
    {
        [JsonProperty( PropertyName = "stat" )]
        public Status Status { get; set; }

        [JsonProperty( PropertyName = "account" )]
        public Account Account { get; set; }

        [JsonProperty( PropertyName = "error" )]
        public Error Error { get; set; }
    }
}
