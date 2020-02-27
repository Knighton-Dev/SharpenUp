using System;
using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;

namespace SharpenUp.Common.Models.Monitors
{
    [ExcludeFromCodeCoverage]
    public class ResponseTime
    {
        [JsonProperty( PropertyName = "datetime" )]
        public int IntervalTimeInt { get; set; }

        public DateTime IntervalTime
        {
            get
            {
                DateTimeOffset offset = DateTimeOffset.FromUnixTimeSeconds( IntervalTimeInt );
                return offset.UtcDateTime;
            }
        }

        [JsonProperty( PropertyName = "value" )]
        public int Value { get; set; }
    }
}
