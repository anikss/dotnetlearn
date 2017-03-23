using Newtonsoft.Json;

namespace WeatherApp
{
    /// <summary>
    /// Координаты
    /// </summary>
    public class Location
    {
        /// <summary>
        /// Широта
        /// </summary>
        [JsonProperty("lat")]
        public string Lat { get; set; }

        /// <summary>
        /// Долгота́ 
        /// </summary>
        [JsonProperty("lng")]
        public string Lng { get; set; }

        /// <summary>
        /// Долгота́ 
        /// </summary>
        [JsonProperty("formatted_address")]
        public string Formatted_address { get; set; }
    }
}