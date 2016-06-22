using System;
using System.Web.Http;
using Util;

namespace Live_Performance.Controllers
{
    public class TemperatureController : ApiController
    {
        /// <summary>
        ///     Calculate the wind chill for a given day.
        /// </summary>
        /// <param name="date">The day for which to calculate the wind chill.</param>
        /// <returns>The wind chill.</returns>
        public double Index(DateTime date)
        {
            double temperature = 0;
            double wind = 0;

            return Temperature.WindChill(temperature, wind);
        }
    }
}
