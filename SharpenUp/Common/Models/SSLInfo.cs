using System;
namespace SharpenUp.Common.Models
{
    // TODO: JsonProperties
    public class SSLInfo
    {
        public string Brand { get; set; }
        public string Product { get; set; }
        public int Expires { get; set; } // TODO: Convert to DateTime
        public bool IgnoreErrors { get; set; } // TODO: May not map
        public bool DisbaleNotifications { get; set; }
    }
}
