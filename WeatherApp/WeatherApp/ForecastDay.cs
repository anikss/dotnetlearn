using Newtonsoft.Json;

namespace WeatherApp
{
    /// <summary>
    /// Прогноз погоды на день
    /// </summary>
    internal class ForecastDay
    {
        /// <summary>
        /// Наивысшая температура дня
        /// </summary>
        [JsonProperty("high")]
        public Degree High { get; set; }

        /// <summary>
        /// Наинизшая температура дня
        /// </summary>
        [JsonProperty("low")]
        public Degree Low { get; set; }

        /// <summary>
        /// Погодные условия
        /// </summary>
        [JsonProperty("conditions")]
        public string Conditions { get; set; }
    }
}
