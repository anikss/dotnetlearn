using System;
using System.CodeDom.Compiler;
using System.Collections;
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
            string marketData = "";
            IList<Result> resultsDataOut = new List<Result>();
            int i = 0;
            float hAlert = 0;
            float lAlert = 0;

            Console.WriteLine("Insert Hight Alert: ");
            hAlert = float.Parse(Console.ReadLine());
            //Console.WriteLine(bidAlert);

            Console.WriteLine("Insert low Alert: ");
            lAlert = float.Parse(Console.ReadLine());
            //Console.WriteLine(askAlert);

            do
            {
                while (!Console.KeyAvailable)
                {
                    DownloadMarketSummWaves(out marketData);
                    resultsDataOut = ParsApi(marketData);
                    PrintResult(resultsDataOut, hAlert, lAlert);
                    CheckAskBid(resultsDataOut, ref hAlert, ref lAlert);                  
                }

            } while (Console.ReadKey(true).Key != ConsoleKey.Escape);

           // Console.ReadLine();

        }

        private static void DownloadMarketSummWaves(out string marketData)
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
                marketData = Encoding.Default.GetString(data);
                //Console.WriteLine(marketData);



                //Result result = JsonConvert.DeserializeObject<Result>(marketData, SerializerSettings);
                // Console.WriteLine(result.MarketName);
                // Console.WriteLine(result.Last);
            }
            else
            {
                marketData = "Ошибка API!";
            }
            /*               var results = ParseResults(marketData);
                           Console.WriteLine(results[0]);
                       }
                   }
           
                   private static List<Result> ParseResults(string marketData)
                   {
                       var parsed = JObject.Parse(marketData);
                       return parsed["MarketName"]["Last"]
                           .Children()
                           .Select(item => JsonConvert.DeserializeObject<Result>(item.ToString(), SerializerSettings))
                           .ToList();
                           */   
        }

        private static IList<Result> ParsApi(string marketData)
        {
            JObject resultData = JObject.Parse(marketData);
            IList<JToken> results = resultData["result"].Children().ToList();
            IList<Result> resultsData = new List<Result>();
            foreach (JToken resultIND in results)
            {
                Result result = JsonConvert.DeserializeObject<Result>(resultIND.ToString());
                resultsData.Add(result);
            }
            return resultsData;
        }
        private static void PrintResult(IList<Result> resultsData, float hAlert, float lAlert)
        {
            if (resultsData == null) throw new ArgumentNullException(nameof(resultsData));
            Console.Clear();
            Console.WriteLine($"MarketName: " +
                              $"{resultsData[0].MarketName}");
            Console.WriteLine($"High: " +
                              $"{resultsData[0].High}");
            Console.WriteLine($"Low: " +
                              $"{resultsData[0].Low}");
            Console.WriteLine($"Volume: " +
                              $"{resultsData[0].Volume}");
            Console.WriteLine($"BaseVolume: " +
                              $"{resultsData[0].BaseVolume}");
            Console.WriteLine($"TimeStamp: " +
                              $"{resultsData[0].TimeStamp}");
            Console.WriteLine($"Bid: " +
                              $"{resultsData[0].Bid}");
            Console.WriteLine($"Ask: " +
                              $"{resultsData[0].Ask}");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Last: " +
                   $"{resultsData[0].Last}");
            Console.ResetColor();
            Console.WriteLine($"Hight Alert: " + $"{hAlert}");
            Console.WriteLine($"Low Alert: " + $"{lAlert}");
            Thread.Sleep(3000);
            
        }

        private static void CheckAskBid(IList<Result> resultsData, ref float hAlert, ref float lAlert)
        {
            float last = float.Parse(resultsData[0].Last, System.Globalization.CultureInfo.CreateSpecificCulture("en-US"));
            if (last >= hAlert)
            {
                Console.Clear();
                Console.WriteLine($"Your Hight alert: {hAlert}");
                Console.WriteLine($"Last price when alert on: {last}");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Wait...");
                Console.ResetColor();
                Mario();
                Console.WriteLine("Insert New Hight Alert: ");
                hAlert = float.Parse(Console.ReadLine());
            }
            if (last <= lAlert)
            {
                Console.WriteLine($"Your Low alert: {lAlert}");
                Console.WriteLine($"Last price when alert on: {last}");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Wait...");
                Console.ResetColor();
                Mario();
                Console.WriteLine("Insert New Low Alert: ");
                lAlert = float.Parse(Console.ReadLine());
            }
        }

        private static void Mario()
        {
            Console.Beep(659, 125);
            Console.Beep(659, 125);
            Thread.Sleep(125);
            Console.Beep(659, 125);
            Thread.Sleep(167);
            Console.Beep(523, 125);
            Console.Beep(659, 125);
            Thread.Sleep(125);
            Console.Beep(784, 125);
            Thread.Sleep(375);
            Console.Beep(392, 125);
            Thread.Sleep(375);
            Console.Beep(523, 125);
            Thread.Sleep(250);
            Console.Beep(392, 125);
            Thread.Sleep(250);
            Console.Beep(330, 125);
            Thread.Sleep(250);
            Console.Beep(440, 125);
            Thread.Sleep(125);
            Console.Beep(494, 125);
            Thread.Sleep(125);
            Console.Beep(466, 125);
            Thread.Sleep(42);
            Console.Beep(440, 125);
            Thread.Sleep(125);
            Console.Beep(392, 125);
            Thread.Sleep(125);
            Console.Beep(659, 125);
            Thread.Sleep(125);
            Console.Beep(784, 125);
            Thread.Sleep(125);
            Console.Beep(880, 125);
            Thread.Sleep(125);
            Console.Beep(698, 125);
            Console.Beep(784, 125);
            Thread.Sleep(125);
            Console.Beep(659, 125);
            Thread.Sleep(125);
            Console.Beep(523, 125);
            Thread.Sleep(125);
            Console.Beep(587, 125);
            Console.Beep(494, 125);
            Thread.Sleep(125);
            Console.Beep(523, 125);
            Thread.Sleep(250);
            Console.Beep(392, 125);
            Thread.Sleep(250);
            Console.Beep(330, 125);
            Thread.Sleep(250);
            Console.Beep(440, 125);
            Thread.Sleep(125);
            Console.Beep(494, 125);
            Thread.Sleep(125);
            Console.Beep(466, 125);
            Thread.Sleep(42);
            Console.Beep(440, 125);
            Thread.Sleep(125);
            Console.Beep(392, 125);
            Thread.Sleep(125);
            Console.Beep(659, 125);
            Thread.Sleep(125);
            Console.Beep(784, 125);
            Thread.Sleep(125);
            Console.Beep(880, 125);
            Thread.Sleep(125);
            Console.Beep(698, 125);
            Console.Beep(784, 125);
            Thread.Sleep(125);
            Console.Beep(659, 125);
            Thread.Sleep(125);
            Console.Beep(523, 125);
            Thread.Sleep(125);
            Console.Beep(587, 125);
            Console.Beep(494, 125);
            Thread.Sleep(375);
            Console.Beep(784, 125);
            Console.Beep(740, 125);
            Console.Beep(698, 125);
            Thread.Sleep(42);
            Console.Beep(622, 125);
            Thread.Sleep(125);
            Console.Beep(659, 125);
            Thread.Sleep(167);
            Console.Beep(415, 125);
            Console.Beep(440, 125);
            Console.Beep(523, 125);
            Thread.Sleep(125);
            Console.Beep(440, 125);
            Console.Beep(523, 125);
            Console.Beep(587, 125);
            Thread.Sleep(250);
            Console.Beep(784, 125);
            Console.Beep(740, 125);
            Console.Beep(698, 125);
            Thread.Sleep(42);
            Console.Beep(622, 125);
            Thread.Sleep(125);
            Console.Beep(659, 125);
            Thread.Sleep(167);
            Console.Beep(698, 125);
            Thread.Sleep(125);
            Console.Beep(698, 125);
            Console.Beep(698, 125);
            Thread.Sleep(625);
            Console.Beep(784, 125);
            Console.Beep(740, 125);
            Console.Beep(698, 125);
            Thread.Sleep(42);
            Console.Beep(622, 125);
            Thread.Sleep(125);
            Console.Beep(659, 125);
            Thread.Sleep(167);
            Console.Beep(415, 125);
            Console.Beep(440, 125);
            Console.Beep(523, 125);
            Thread.Sleep(125);
            Console.Beep(440, 125);
            Console.Beep(523, 125);
            Console.Beep(587, 125);
            Thread.Sleep(250);
            Console.Beep(622, 125);
            Thread.Sleep(250);
            Console.Beep(587, 125);
            Thread.Sleep(250);
            Console.Beep(523, 125);
            Thread.Sleep(1125);
            Console.Beep(784, 125);
            Console.Beep(740, 125);
            Console.Beep(698, 125);
            Thread.Sleep(42);
            Console.Beep(622, 125);
            Thread.Sleep(125);
            Console.Beep(659, 125);
            Thread.Sleep(167);
            Console.Beep(415, 125);
            Console.Beep(440, 125);
            Console.Beep(523, 125);
            Thread.Sleep(125);
            Console.Beep(440, 125);
            Console.Beep(523, 125);
            Console.Beep(587, 125);
            Thread.Sleep(250);
            Console.Beep(784, 125);
            Console.Beep(740, 125);
            Console.Beep(698, 125);
            Thread.Sleep(42);
            Console.Beep(622, 125);
            Thread.Sleep(125);
            Console.Beep(659, 125);
            Thread.Sleep(167);
            Console.Beep(698, 125);
            Thread.Sleep(125);
            Console.Beep(698, 125);
            Console.Beep(698, 125);
            Thread.Sleep(625);
            Console.Beep(784, 125);
            Console.Beep(740, 125);
            Console.Beep(698, 125);
            Thread.Sleep(42);
            Console.Beep(622, 125);
            Thread.Sleep(125);
            Console.Beep(659, 125);
            Thread.Sleep(167);
            Console.Beep(415, 125);
            Console.Beep(440, 125);
            Console.Beep(523, 125);
            Thread.Sleep(125);
            Console.Beep(440, 125);
            Console.Beep(523, 125);
            Console.Beep(587, 125);
            Thread.Sleep(250);
            Console.Beep(622, 125);
            Thread.Sleep(250);
            Console.Beep(587, 125);
            Thread.Sleep(250);
            Console.Beep(523, 125);
            Thread.Sleep(625);
        }
    }
}
