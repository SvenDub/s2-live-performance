using System;

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
    }
}