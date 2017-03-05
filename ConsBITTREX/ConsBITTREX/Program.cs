using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ConsBITTREX
{
    class Program
    {
        private static readonly JsonSerializerSettings SerializerSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            MissingMemberHandling = MissingMemberHandling.Ignore
        };
        static void Main(string[] args)
        {
            DownloadMarketSummWaves();
            Console.ReadLine();
        }

        static void DownloadMarketSummWaves()
        {
            const string url = "https://bittrex.com/api/v1.1/public/getmarketsummary?market=btc-waves";
            WebClient client = new WebClient();
            byte[] data = null;

            try
            {
                data = client.DownloadData(new Uri(url));
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp.ToString());
            }

            if (data != null)
            {
                string marketData = Encoding.Default.GetString(data);
                Console.WriteLine(marketData);
                var results = ParseResults(marketData);
                Console.WriteLine(results[0]);
            }
        }

        private static List<result> ParseResults(string marketData)
        {
            var parsed = JObject.Parse(marketData);
            return parsed["MarketName"]["Last"]
                .Children()
                .Select(item => JsonConvert.DeserializeObject<result>(item.ToString(), SerializerSettings))
                .ToList();

        }
    }
}
