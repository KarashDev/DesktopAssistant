using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DesktopAssistant.API_weather_response
{
    static class WeatherDataHandler
    {
        // Написано под десериализацию JSON с API Яндекс Погода
        public class JsonWeatherMainTable
        {
            public JsonWeatherInnerTable_fact fact { get; set; }
            public JsonWeatherInnerTable_forecast forecast { get; set; }
        }

        public class JsonWeatherInnerTable_fact
        {
            public float temp { get; set; }
            public float feels_like { get; set; }
            public float wind_speed { get; set; }
        }
        public class JsonWeatherInnerTable_forecast
        {
            public Parts[] parts { get; set; }
        }
        
        public class Parts
        {
            public float temp_max { get; set; }
            public float temp_min { get; set; }
        }


        static string jsonResponseFromServer;

       
        // Конкретные десериализированные данные, которые отправляются к контролам формы
        public static float temp_max;
        public static float temp_min;


        public static async Task GetApiJsonData()
        {
            // Срок прогноза в днях (limit) не работает
            var weatherApiUrl = String.Format(CultureInfo.InvariantCulture, "https://api.weather.yandex.ru/v2/informers?lat={0}&lon={1}", 51.46, 55.06);

            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(weatherApiUrl);

            httpWebRequest.Headers["X-Yandex-API-Key"] = "34082f29-3bb2-4404-b057-1995f3441697";

            HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();

            using (StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream()))
            {
                jsonResponseFromServer = streamReader.ReadToEnd();
            }

            var weatherResponse = JsonConvert.DeserializeObject<WeatherDataHandler.JsonWeatherMainTable>(jsonResponseFromServer);
            
            // [0] - это данные днем (не ночью)
            temp_max = weatherResponse.forecast.parts[0].temp_max;
            // [1] - это данные ночью (не днем)
            temp_min = weatherResponse.forecast.parts[1].temp_min;
        }

        // На случай, если API присылает XML
        //static void GetApiXmlData() { }
    }
}
