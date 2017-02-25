using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Http.Headers;
using System.Net.NetworkInformation;
using System.Security.Cryptography.X509Certificates;
using System.Threading;


namespace WeatherApp
{
    class Program
    {
        static ManualResetEvent evnts = new ManualResetEvent(false); // Глобальная переменная для загрузки?
        static void Main(string[] args)
        {
            TestInternetConnection(); // Проверка соединения
            // Запрос названия города у пользователя
            // Отправка запроса погоды для выбранного города (запрос может вернуть: ошибку в названии города, ошибку соединения или вывести данные о погоде)
            // Запрашиваем пользователя, хочет ли он продолжить?

            DownloadInfoAboutWeather_test1(); // Тест загрузки информации о погоде
            DownloadInfoAboutWeather_test2();
            Console.ReadLine();
        }

        static void TestInternetConnection() //Проверка интернет соединения
        {
            string pingAdress = "google.com"; // Задаем адрес для тестирования соединения
            int pingDeley = 12000; // Задаем задержку для ping
            Ping testPing = new Ping();
            testPing.PingCompleted += new PingCompletedEventHandler(PingCompletedCallback); // Ловим эвент?
            testPing.SendPingAsync(pingAdress, pingDeley); // Посылаем пинг

        }

        private static void PingCompletedCallback(object sender, PingCompletedEventArgs pingEventArgs)
        {
            if (pingEventArgs.Error != null)  // Если плйманый эвент с ошибкой
            {
                Console.WriteLine("No internet connection"); // Выводим сообщение, что нет соединения
                Console.WriteLine(pingEventArgs.Error.ToString()); // Печатаем ошибку 
            }

            PingReply reply = pingEventArgs.Reply; // Если ошибки не было, то получаем ответ
            DisplayReply(reply); // Выводим ответ


        }

        public static void DisplayReply(PingReply reply)
        {
            if (reply == null)
                return;
            Console.WriteLine("ping status {0}", reply.Status);
            if (reply.Status == IPStatus.Success)
            {
                Console.WriteLine("Adress: {0}", reply.Address.ToString());
                Console.WriteLine("RoadTrip time {0}", reply.RoundtripTime);
                Console.WriteLine("To to live: {0}", reply.Options.Ttl);
                Console.WriteLine("Don't fragment: {0}", reply.Options.DontFragment);
                Console.WriteLine("Buffer size: {0}", reply.Buffer.Length);
            }
        }


        static void DownloadInfoAboutWeather_test1 () // Класс для загрузки информации о погоде
        {
            byte[] data = null; // Массив с байтами для определения размера скаченного? В итоге туда влезло все скаченное.
            WebClient client = new WebClient(); // Инициализируем новый экземпляр метода WebClient, для скачивания информации по URL
            client.DownloadDataCompleted +=
                delegate (object sender, DownloadDataCompletedEventArgs e)
                {
                    data = e.Result;
                    evnts.Set();
                };
            Console.WriteLine("starting...");
            evnts.Reset();
            client.DownloadDataAsync(new Uri("http://api.wunderground.com/api/da7c79da65c82cdf/geolookup/conditions/q/IA/Cedar_Rapids.json"));
            evnts.WaitOne(); // wait to download complete

            //string str = Encoding.Default.GetString(data); // Пробуем вставить результат в строку, для этого преобразуем массив символов в строку. Можно было скачать сразу все в строку, но того поломается счетчик.

            Console.WriteLine("done. {0} bytes received;", data.Length);
           // Console.WriteLine("{0}",str);
        }

        static void DownloadInfoAboutWeather_test2()
        {
            string url = "http://api.wunderground.com/api/da7c79da65c82cdf/conditions/q/CA/San_Francisco.json";
            WebClient client = new WebClient();
            //client.DownloadDataCompleted += DownloadDataCompleted;
            //client.DownloadDataAsync(new Uri(url));
            client.DownloadStringCompleted += DownloadStringCompletd;
            client.DownloadStringAsync(new Uri(url));
            Console.ReadLine();
        }

        static void DownloadDataCompleted(object sender,
    DownloadDataCompletedEventArgs e)
        {
            byte[] raw = e.Result;
            Console.WriteLine(raw.Length + " bytes received");
        }

        static void DownloadStringCompletd(object sender, DownloadStringCompletedEventArgs weatherEventArgs)
        {
            string WeatherJSON = weatherEventArgs.Result;
            Console.WriteLine(WeatherJSON);
        }
    }
}
