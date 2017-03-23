using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WeatherApp
{
    public class Weather
    {
        private const string googleMapApiGeo = "http://maps.googleapis.com/maps/api/geocode/";
        private const string apiWeatherUrl = "http://api.wunderground.com/api/";
        private const string langWApi = "lang:RU/";
        private const string key = "da7c79da65c82cdf";

        #region Настроки JsonSerializerSettings
        private static readonly JsonSerializerSettings SerializerSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            MissingMemberHandling = MissingMemberHandling.Ignore
        };
        #endregion

        #region Преобразование названия города в координаты
        public static Location TownNameToCoordinate(string townName)
        {
            string apiUrl = googleMapApiGeo +
                            "json?address=" +
                            townName +
                            "&sensor=false&language=ru";
            string jsonData = "";

            #region Загрузка данных с сервера google
            try // Загружаем данные в массив, потом записываем в строку
            {
                WebClient client = new WebClient();
                byte[] data = client.DownloadData(new Uri(apiUrl)); 
                jsonData = data != null ? Encoding.UTF8.GetString(data) : "Ошибка API!";
            }
            catch (Exception exception) // Ловим исключение, если API не работает и выводим ошибку.
            {
                Console.WriteLine("Проблемы с API Google. Попробуйте еще раз. Подробная информация:");
                Console.WriteLine(exception.Message);
            }
            #endregion

            dynamic json = JsonConvert.DeserializeObject(jsonData);
            Location location = new Location
            {
                Lat = json.results[0].geometry.location.lat,
                Lng = json.results[0].geometry.location.lng,
                Formatted_address = json.results[0].formatted_address
            };
            return location;
        }
        #endregion
 
        #region Отображение данных о погоде в городе
        public async Task PrintWeatherInTown(string townName)
        {
            Location location = TownNameToCoordinate(townName);
            string coordinate = location.Lat + "," + location.Lng;
            string apiUrl = apiWeatherUrl + key + "/forecast/" + langWApi + "q/" + coordinate + ".json";
            string jsonData = "";

            #region Загрузка погодных данных
            try // Загружаем данные в массив, потом записываем в строку
            {
                WebClient client = new WebClient();
                byte[] data = client.DownloadData(new Uri(apiUrl)); ;
                jsonData = data != null ? Encoding.UTF8.GetString(data) : "Ошибка API!";
            }
            catch (Exception exception) // Ловим исключение, если API не работает и выводим ошибку.
            {
                Console.WriteLine("Проблемы с API погоды. Попробуйте еще раз. Подробная информация:");
                Console.WriteLine(exception.Message);
            }
            #endregion

            var forecast = ParseForecastDays(jsonData);
            // Выводим погоду на сегодня и на завтра.
            // Для простоты мы не парсим даты из JSON и пользуемся тем, что они идут всегда по порядку друг за другом, начиная с сегодняшней.
            var today = forecast[0];
            var tomorrow = forecast[1];
            Console.WriteLine("------------------------------------------------------");
            Console.WriteLine(location.Formatted_address);
            Console.WriteLine($"Температура сегодня: {today.High}/{today.Low} ({today.Conditions})");
            Console.WriteLine($"Завтра: {tomorrow.High}/{tomorrow.Low} ({tomorrow.Conditions})");
            Console.WriteLine("------------------------------------------------------");
        }
        private static List<ForecastDay> ParseForecastDays(string jsonData)
        {
            var parsed = JObject.Parse(jsonData);
            return parsed["forecast"]["simpleforecast"]["forecastday"]
                .Children()
                .Select(item => JsonConvert.DeserializeObject<ForecastDay>(item.ToString(), SerializerSettings))
                .ToList();
        }
        #endregion
    }
}