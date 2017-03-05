using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ConsBITTREX
{
    internal class Result
    {
        [JsonProperty("MarketName")]
        public string MarketName { get; set; }

        [JsonProperty("High")]
        public string High { get; set; }

        [JsonProperty("Low")]
        public string Low { get; set; }

        [JsonProperty("Volume")]
        public string Volume { get; set; }

        [JsonProperty("Last")]
        public string Last { get; set; }

        [JsonProperty("BaseVolume")]
        public string BaseVolume { get; set; }

        [JsonProperty("TimeStamp")]
        public string TimeStamp { get; set; }

        [JsonProperty("Bid")]
        public string Bid { get; set; }

        [JsonProperty("Ask")]
        public string Ask { get; set; }

        [JsonProperty("OpenBuyOrders")]
        public string OpenBuyOrders { get; set; }

        [JsonProperty("OpenSellOrders")]
        public string OpenSellOrders { get; set; }

        [JsonProperty("PrevDay")]
        public string PrevDay { get; set; }

    }
}
