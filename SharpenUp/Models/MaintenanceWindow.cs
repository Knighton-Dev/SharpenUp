using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace SharpenUp.Models
{
    public class BaseMaintenanceWindow
    {
        /// <summary>
        /// The ID of the maintenance window.
        /// </summary>
        [JsonProperty( PropertyName = "id" )]
        public int? Id { get; set; }
    }

    public class MaintenanceWindow : BaseMaintenanceWindow
    {
        /// <summary>
        /// The type of the maintenance window.
        /// </summary>
        [JsonProperty( PropertyName = "type" )]
        public MaintenanceWindowType? MaintenanceWindowType { get; set; }

        /// <summary>
        /// Friendly name of the maintenance window (for making it easier to distinguish from others).
        /// </summary>
        [JsonProperty( PropertyName = "friendly_name" )]
        public string FriendlyName { get; set; }

        /// <summary>
        /// Seperated with "-" and used only for weekly and monthly maintenance windows.
        /// </summary>
        [JsonProperty( PropertyName = "value" )]
        public string Value { get; set; }

        /// <summary>
        /// Start time of the maintenance windows.
        /// </summary>
        [JsonProperty( PropertyName = "start_time" )]
        private string StartTimeString { get; set; }

        public TimeSpan? StartTime
        {
            get
            {
                if ( string.IsNullOrWhiteSpace( StartTimeString ) )
                {
                    return null;
                }
                else
                {
                    List<string> convertString = StartTimeString.Split( ':' ).ToList();
                    return new TimeSpan( Convert.ToInt32( convertString[ 0 ] ), Convert.ToInt32( convertString[ 1 ] ), 0 );
                }
            }
        }

        /// <summary>
        /// Duration of the maintenance windows in minutes.
        /// </summary>
        [JsonProperty( PropertyName = "duration" )]
        public int? Duration { get; set; }

        /// <summary>
        /// The status of the maintenance window.
        /// </summary>
        [JsonProperty( PropertyName = "status" )]
        public MaintenanceWindowStatus? MaintenanceWindowStatus { get; set; }
    }

    public enum MaintenanceWindowType
    {
        Once = 1,
        Daily = 2,
        Weekly = 3,
        Monthly = 4
    }

    public enum MaintenanceWindowStatus
    {
        Paused = 0,
        Active = 1
    }
}
