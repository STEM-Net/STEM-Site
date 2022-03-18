using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STEM_Net.Services
{
    public interface IWeatherService
    {
        /// <summary>
        /// Provides the recent average precipitation at the given location.
        /// </summary>
        /// <param name="longitude">The longitude of the location for precipitation data.</param>
        /// <param name="latitude">The latitude of the location for precipitation data.</param>
        /// <param name="hoursPrior">The number of prior hours to average precipitation over.</param>
        /// <returns></returns>
        Task<double> GetPrecipitationAsync(double longitude, double latitude, int hoursPrior);

        /// <summary>
        /// Provides the recent average precipitation at the given location.
        /// </summary>
        /// <param name="longitude">The longitude of the location for precipitation data.</param>
        /// <param name="latitude">The latitude of the location for precipitation data.</param>
        /// <returns></returns>
        Task<double> GetPrecipitationAsync(double longitude, double latitude);

        /// <summary>
        /// Provides a list of precipitation values over the past given number of hours.
        /// </summary>
        /// <param name="longitude">The longitude of the location for precipitation data.</param>
        /// <param name="latitude">The latitude of the location for precipitation data.</param>
        /// <param name="hoursPrior">The number of prior hours to average precipitation over.</param>
        /// <returns></returns>
        Task<List<double>> GetHourlyPrecipitationAsync(double longitude, double latitude, int hoursPrior);

        /// <summary>
        /// Provides the current temperature at the given location.
        /// </summary>
        /// <param name="longitude">The longitude of the location for temperature data.</param>
        /// <param name="latitude">The latitude of the location for the temperature data.</param>
        /// <returns></returns>
        Task<double> GetTemperatureAsync(double longitude, double latitude);
    }
}
