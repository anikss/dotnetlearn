using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Net;
using System.Timers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TidexWCF.Model;

namespace TidexWCF
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change tzhe class name "MarketBotService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select MarketBotService.svc or MarketBotService.svc.cs at the Solution Explorer and start debugging.
    public class MarketBotService : IMarketBotService
    {
        public decimal lasNow;
        public Timer timer;
        public MarketBotService()
        {
            timer = new Timer(1000 * 10);
            timer.Elapsed += WCFService_Elapsed;
            timer.Start();
        }

        void WCFService_Elapsed(object sender, ElapsedEventArgs e)
        {
            //MarketData m = GetLastPrice("waves_btc");
            lasNow = lasNow++;
        }



        #region Common Methods
        /// <summary>
        /// проверка соединения
        /// </summary>
        /// <returns> OK </returns>
        public string TestConnection()
        {
            return "OK";
        }
        #endregion

        public decimal GetData()
        {
            return lasNow;
        }

        #region Get last price
        /// <summary>
        /// Возвращает цену последней продажи на рынке, входное значение - название рынка, например "waves_btc"
        /// </summary>
        /// <returns> decimal </returns>
        public MarketData GetLastPrice(string marketName)
        {
            var apiUrl = "https://api.tidex.com/api/3/ticker/"+ marketName;
            MarketData market = new MarketData();
            string jsonData;

            try // Загружаем данные в массив, потом записываем в строку
            {
                WebClient client = new WebClient();
                byte[] data = client.DownloadData(new Uri(apiUrl));
                jsonData = data != null ? Encoding.UTF8.GetString(data) : "Ошибка API!";
            }
            catch (Exception exception) // Ловим исключение, если API не работает и выводим ошибку.
            {
                return market;
            }

            var parsed = JObject.Parse(jsonData);
            var token = parsed[marketName];
            market.Last = token["last"].Value<decimal>();
            market.Buy = token["buy"].Value<decimal>();
            market.Sell = token["sell"].Value<decimal>();
            market.Avg = token["avg"].Value<decimal>();
            market.High = token["high"].Value<decimal>();
            market.Low = token["low"].Value<decimal>();
            market.VolCur = token["vol_cur"].Value<decimal>(); 
            market.MarketName = marketName;
            market.Updated = token["updated"].Value<string>();
            return market;
        }
        #endregion

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }
    }
}
