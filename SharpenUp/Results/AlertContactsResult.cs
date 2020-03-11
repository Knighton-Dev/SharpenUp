using System.Collections.Generic;
using Newtonsoft.Json;
using SharpenUp.Models;

namespace SharpenUp.Results
{
    public class AlertContactsResult : BaseResult
    {
        [JsonProperty( PropertyName = "alertcontact" )]
        public BaseAlertConctact BaseAlertContact { get; set; }

        [JsonProperty( PropertyName = "alert_contacts" )]
        public List<AlertContact> AlertContacts { get; set; }
    }
}
