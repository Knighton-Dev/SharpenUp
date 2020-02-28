using System;
using Newtonsoft.Json;

namespace SharpenUp.Models
{
    public class Log
    {
        /// <summary>
        /// The value of the keyword.
        /// </summary>
        [JsonProperty( PropertyName = "type" )]
        public LogType LogType { get; set; }

        /// <summary>
        /// The date and time of the log (inherits the user's timezone setting).
        /// </summary>
        [JsonProperty( PropertyName = "datetime" )]
        public DateTime DateTime { get; set; }

        /// <summary>
        /// The duration of the downtime in seconds.
        /// </summary>
        [JsonProperty( PropertyName = "duration" )]
        public int Duration { get; set; }

        /// <summary>
        /// The reason of the downtime (if exists).
        /// </summary>
        [JsonProperty( PropertyName = "reason" )]
        public string Reason { get; set; }
    }

    public enum LogType
    {
        Down = 1,
        Up = 2,
        Paused = 99,
        Started = 98
    }
}
