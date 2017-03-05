using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ConsBITTREX
{
    internal class result
    {
        [JsonProperty("MarketName")]
        public string MarketName { get; set; }

        [JsonProperty("Last")]
        public int Last { get; set; }

    }
}
