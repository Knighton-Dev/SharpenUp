using System;
using SharpenUp.Common.Types;
using Newtonsoft.Json;

namespace SharpenUp.Common.Models
{
    public class AccountDetails
    {
        [JsonProperty( PropertyName = "stat" )]
        public StatusType Status { get; set; }

        [JsonProperty( PropertyName = "error" )]
        public Error Error { get; set; }

        [JsonProperty( PropertyName = "account" )]
        public Account Account { get; set; }
    }
}
