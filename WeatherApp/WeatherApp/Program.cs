using System;
using System.Data;
using System.Text;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WeatherApp
{
    class Program
    {
        const string apiWeatherUrl = "http://api.wunderground.com/api/da7c79da65c82cdf/";
        const string langApi = "lang:RU/";
        static void Main(string[] args)
        {
            const int internetAttempt = 20;
            bool townExistStatus = new bool();
            bool internetStatus = new bool();
            string townLink ="";
            string replyStatus;
            string townName = "";

            // Проверка соединения.
            int i = internetAttempt;  //Количество попыток соединений.
            do
            {
                if (i==0) // Если попытки кончились, то выходим из цикла.
                    break;
                internetStatus = TestInternetConnection(out replyStatus);
                Console.WriteLine(replyStatus);
                if (internetStatus == false)
                    Console.WriteLine($"Attempt left: {i}");
                i--;

            } while (!internetStatus);

            // Запрос названия города у пользователя.
            // Проверка названия города.

            do
            {
                if (i == 0) break; // Если попытки подключения исчерпаны, то пропускаем цикл.

                Console.WriteLine("Введите город и страну через запятую на английском языке, \n" +
                                  "например, \"Moscow, Russia\". \n" +
                                  "Для выхода наберите \"Exit\" или \"E\".");
                townName = Console.ReadLine();
                // Если пользователь пише exit или e то выходим из цикла.
                if ((townName.ToLower() == "exit") || (townName.ToLower() == "e"))
                    break;
                // Проверяем название города.
                CheckTownName(townName, out townExistStatus, out townLink);
                //Console.WriteLine(townExistStatus);

            } while (!townExistStatus); // Выходим, если получили одно точное соответсвие.

            // Отправка запроса погоды для выбранного города.
            if ((townName.ToLower() != "exit") && (townName.ToLower() != "e"))
                 CheckWeather(townLink);

            Console.WriteLine("\nОтлично поработали!");
            Console.ReadLine();
        }

        #region Проверка интернет соединения
        private static bool TestInternetConnection(out string replyStatus) //Проверка интернет соединения.
        {
            // Параметры тестирования
            const string pingAddress = "google.com"; // Задаем адрес для тестирования соединения.
            const int pingDelay = 2000; // Задаем задержку для ping, мс.
            Ping testPing = new Ping(); // Создаем новый экземляр Ping.
            
            // Посылаем пинг, передаем адрес и задержку, получаем ответ в reply.
            PingReply reply = null;
            try
            {
               reply = testPing.Send(pingAddress, pingDelay);
            }
            // Ловим исключение, если интернет отключен аппаратно.
            catch (Exception)
            {
                replyStatus = "Нет доступа в интернет. Пожалуйста, подключите интернет. \n" +
                              "Пытаемся переподключиться...";
                Thread.Sleep(3000); //Засымаем, мс.
                return false;
            }
            // Если интернет есть, то возвращаем true.
            if (reply.Status == IPStatus.Success)
            {
                replyStatus = "Интеренет доступен.";
                return true;
            }
            // Если инетернета нет, но он подключен аппаратно, возвращаем false.
            else
            {
                replyStatus = "Возьникли неполадки с доступом к тестовому сайту. \n" +
                              reply.Status.ToString() + ".\n" +
                              "Пытаемся получить доступ еще раз...";
                Thread.Sleep(1200); //Засымаем, мс.
                return false;
            }
        }
        #endregion

        #region Проверка наличия места
        private static void CheckTownName(string townName, out bool townExistStatus, out string townLink) // Проверка названия места.
        {
            // Апи для проверки страны и города.
            const string autocompleteUrl = "http://autocomplete.wunderground.com/aq?query="; 
            string url = autocompleteUrl + townName; // Формируем ссылку с городом.
            WebClient client = new WebClient();
            byte[] data = null; // Сюда придет ответ от API.

            try
            {
                data = client.DownloadData(new Uri(url)); // Загружаем данные в массив.
            }
            catch (Exception exception) // Ловим исключение, если API не работает.
            {
                Console.WriteLine("Проблемы с API. Попробуйте еще раз.");
                //Console.WriteLine(exception.Message.ToString());
            }
            if (data != null) // Если данные загрузились
            {
                string jsonData = Encoding.Default.GetString(data); // Кодируем массив в строку.
                //Создаем DataSet для json
                DataSet townDataSet = JsonConvert.DeserializeObject<DataSet>(jsonData);
                //Создаем таблицу и заполняем ее значениями из json (RESULTS).
                DataTable townDataTable = townDataSet.Tables["RESULTS"]; 
                Console.WriteLine();
                foreach (DataRow townRow in townDataTable.Rows)
                {
                    Console.WriteLine(townRow["name"]);
                }
                // Возвращаем true, только если совпадение однозначное!
                if (townDataTable.Rows.Count == 1)
                {
                    townExistStatus = true;
                    DataRow linkRow = townDataTable.Rows[0];
                    townLink = linkRow["l"].ToString();
                }
                else
                {
                    townExistStatus = false;
                    townLink = "";
                }
            }
            else // Если данные не загрузились, в т.ч. если ответ пустой.
            {
                townExistStatus = false;
                townLink = "";
            }
        }
        #endregion

        #region Запрос данных о погоде

        private static void CheckWeather(string townLink)
        {
            WebClient client = new WebClient();
            byte[] data = null; // Сюда придет ответ от API.
            string url = apiWeatherUrl + "forecast/" + langApi + townLink + ".json";

            try
            {
                data = client.DownloadData(new Uri(url)); // Загружаем данные в массив.
            }
            catch (Exception exception) // Ловим исключение, если API не работает.
            {
                Console.WriteLine("Проблемы с API. Попробуйте еще раз.");
                //Console.WriteLine(exception.Message.ToString());
            }
            if (data != null) // Если данные загрузились
            {
                string jsonData = Encoding.Default.GetString(data); // Кодируем массив в строку.
                //Создаем DataSet для json
                Console.WriteLine(jsonData);
                /*
                DataSet weatherDataSet = JsonConvert.DeserializeObject<DataSet>(jsonData);
                Console.ReadLine();
                //Создаем таблицу и заполняем ее значениями из json (forecast).
                DataTable weatherDataTable = weatherDataSet.Tables["forecast"];
    
                foreach (DataRow weatherRow in weatherDataTable.Rows)
                {
                    Console.WriteLine(weatherRow["title"]);
                    Console.WriteLine(weatherRow["fcttext"]);
                }
                */
            }
        }
        
        #endregion
    }
}
