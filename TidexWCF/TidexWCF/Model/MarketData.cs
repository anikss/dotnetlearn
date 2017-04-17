using System.Collections.Generic;
using Newtonsoft.Json;

namespace TidexWCF.Model
{
    public class MarketData
    {
        public string MarketName { get; set; }
        [JsonProperty("high")]
        public decimal High { get; set; }
        [JsonProperty("low")]
        public decimal Low { get; set; }
        [JsonProperty("avg")]
        public decimal Avg { get; set; }
        [JsonProperty("vol_cur")]
        public decimal VolCur { get; set; }
        [JsonProperty("last")]
        public decimal Last { get; set; }
        [JsonProperty("buy")]
        public decimal Buy { get; set; }
        [JsonProperty("sell")]
        public decimal Sell { get; set; }
        [JsonProperty("updated")]
        public string Updated { get; set; }
    }
}