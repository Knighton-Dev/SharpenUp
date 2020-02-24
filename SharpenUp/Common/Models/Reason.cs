using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;

namespace SharpenUp.Common.Models
{
    [ExcludeFromCodeCoverage]
    public class Reason
    {
        [JsonProperty( PropertyName = "code" )]
        public string Code { get; set; }

        [JsonProperty( PropertyName = "detail" )]
        public string Detail { get; set; }
    }
}
