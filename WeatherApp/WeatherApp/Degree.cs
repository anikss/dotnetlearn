using Newtonsoft.Json;

namespace WeatherApp
{
    /// <summary>
    /// Измерение температуры
    /// </summary>
    internal class Degree
    {
        /// <summary>
        /// Градусы Фаренгейта
        /// </summary>
        [JsonProperty("fahrenheit")]
        public int Fahrenheit { get; set; }

        /// <summary>
        /// Градусы Цельсия
        /// </summary>
        [JsonProperty("celsius")]
        public int Celsius { get; set; }
    }
}
