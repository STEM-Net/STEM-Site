using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STEM_Net.Services
{
    interface IWeatherService
    {
        /// <summary>
        /// Provides the recent average precipitation at the given location.
        /// </summary>
        /// <param name="longitude">The longitude of the location for precipitation data.</param>
        /// <param name="latitude">The latitude of the location for precipitation data.</param>
        /// <param name="hoursPrior">The number of prior hours to average precipitation over.</param>
        /// <returns></returns>
        float GetPrecipitation(float longitude, float latitude, int hoursPrior);

        /// <summary>
        /// Provides the current temperature at the given location.
        /// </summary>
        /// <param name="longitude">The longitude of the location for temperature data.</param>
        /// <param name="latitude">The latitude of the location for the temperature data.</param>
        /// <returns></returns>
        float GetTemperature(float longitude, float latitude);
    }
}
