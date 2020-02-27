using Newtonsoft.Json;
using System;
using SharpenUp.Common.Types;
using System.Diagnostics.CodeAnalysis;

namespace SharpenUp.Common.Models
{
    public class Log
    {
        [JsonProperty( PropertyName = "type" )]
        [ExcludeFromCodeCoverage]
        public LogType LogStatus { get; set; }

        [JsonProperty( PropertyName = "datetime" )]
        private int IncidentTimeInt { get; set; }

        public DateTime IncidentTime
        {
            get
            {
                DateTimeOffset offset = DateTimeOffset.FromUnixTimeSeconds( IncidentTimeInt );
                return offset.UtcDateTime;
            }
        }

        [JsonProperty( PropertyName = "duration" )]
        [ExcludeFromCodeCoverage]
        private int DurationInt { get; set; }

        public TimeSpan Duration
        {
            get
            {
                return TimeSpan.FromSeconds( DurationInt );
            }
        }

        [JsonProperty( PropertyName = "reason" )]
        public Reason Reason { get; set; }
    }
}
