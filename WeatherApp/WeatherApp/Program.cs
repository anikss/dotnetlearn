using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WeatherApp
{
    class Program
    {
        static void Main(string[] args)
        {
            const int internetAttempt = 20;

            // Проверка соединения.
            int i = internetAttempt;  //Количество попыток соединений.
            bool internetStatus;
            do
            {
                if (i==0) // Если попытки кончились, то выходим из цикла.
                    break;
                string replyStatus;
                internetStatus = TestInternetConnection(out replyStatus);
                Console.WriteLine(replyStatus);
                if (internetStatus == false)
                    Console.WriteLine($"Attempt left: {i}");
                i--;

            } while (!internetStatus);
            Console.Write("Введите город или города через пробел: ");
            Weather weather = new Weather();
            string[] townName = Console.ReadLine().Split(' ');

            for (i = 0; i < townName.GetLength(0); i++)
            {
                PrintAll(weather, townName, i);
            }
            Console.ReadKey();
        }

        static async void PrintAll (Weather weather, string[] townName,int i)
        {
                await weather.PrintWeatherInTown(townName[i]);
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

    }
}
