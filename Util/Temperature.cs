using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Util
{
    public class Temperature
    {
        /// <summary>
        ///     Calculate the wind chill.
        /// </summary>
        /// <param name="temperature">The ambient temperature in Celsius.</param>
        /// <param name="wind">The wind speed in m/s.</param>
        /// <returns>The wind chill.</returns>
        public static double WindChill(double temperature, double wind)
        {
            if (wind < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(wind), wind, "Wind speed cannot be negative");
            }
            return Math.Round(33 + (temperature - 33)*(0.474 + 0.454*Math.Sqrt(wind) - 0.0454*wind), 2);
        }

        public static Dictionary<DateTime, double> GetWindChill()
        {
            Dictionary<DateTime, double> chills = new Dictionary<DateTime, double>();

            using (var httpClient = new HttpClient())
            {
                var json =
                    httpClient.GetStringAsync(
                            "http://api.openweathermap.org/data/2.5/forecast?q=Leeuwarden,NL&units=metric&appid=c10147a59e2dbd59fcf9eea1557b2298").Result;
                JObject jsonObject = JObject.Parse(json);

                JArray forecasts = jsonObject["list"].ToObject<JArray>();
                foreach (JObject forecast in forecasts)
                {
                    if (!forecast["wind"].HasValues)
                    {
                        break;
                    }
                    DateTime date = UnixTimeStampToDateTime(forecast["dt"].ToObject<int>());
                    double temp = forecast["main"]["temp"].ToObject<double>();
                    double wind = forecast["wind"]["speed"].ToObject<double>();
                    chills.Add(date, WindChill(temp, wind));
                }

                return chills;
            }
        }

        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }
    }
}