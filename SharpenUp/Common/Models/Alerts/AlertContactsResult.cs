using System.Collections.Generic;
using Newtonsoft.Json;
using SharpenUp.Common.Types;

namespace SharpenUp.Common.Models.Alerts
{
    public class AlertContactsResult
    {
        [JsonProperty( PropertyName = "stat" )]
        public RequestStatusType Status { get; set; }

        [JsonProperty( PropertyName = "error" )]
        public Error Error { get; set; }

        [JsonProperty( PropertyName = "offset" )]
        public int Offset { get; set; }

        [JsonProperty( PropertyName = "limit" )]
        public int Limit { get; set; }

        [JsonProperty( PropertyName = "total" )]
        public int Total { get; set; }

        [JsonProperty( PropertyName = "alert_contacts" )]
        public List<AlertContact> AlertContacts { get; set; }
    }
}
