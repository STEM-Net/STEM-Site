using System;
using System.Text.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace STEM_Net.Services.Implementations
{
    public class WeatherAzureService : IWeatherService
    {
        private HttpClient _client;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Create a new instance of the Azure weather service.
        /// </summary>
        /// <param name="client">HTTP Client passed in via dependency injection</param>
        /// /// <param name="configuration">Configuration API added via dependency injection</param>
        public WeatherAzureService(HttpClient client, IConfiguration configuration)
        {
            _client = client;
            _configuration = configuration;
        }

        private async Task<List<JsonElement>> CallAzureWeatherAPI(double longitude, double latitude, int hoursPrior = 0, bool details = false)
        {
            const string apiVersion = "1.1";
            string urlBase = "https://atlas.microsoft.com/weather/currentConditions/json?";
            string url = urlBase + 
                $"api-version={apiVersion}" +
                $"&query={longitude},{latitude}" +
                $"&unit=imperial" +
                $"&duration={hoursPrior}" +
                $"&duration={details}" +
                $"&subscription-key={_configuration["AzureMapsSubscriptionKey"]}";

            HttpResponseMessage jsonResponse = await _client.GetAsync(url).ConfigureAwait(false);
            string responseBody = await jsonResponse.Content.ReadAsStringAsync().ConfigureAwait(false);

            var deserializedResponse = JsonSerializer.Deserialize<Dictionary<string, List<JsonElement>>>(responseBody);
            return deserializedResponse["results"];
        }

        public async Task<List<double>> GetHourlyPrecipitationAsync(double longitude, double latitude, int hoursPrior)
        {
            CheckValidHoursPrior(hoursPrior);
            List<double> hourlyPrecipitations = new List<double>();
            List<JsonElement> response = await CallAzureWeatherAPI(longitude, latitude, hoursPrior, true).ConfigureAwait(false);
            foreach (JsonElement hourData in response) {
                hourlyPrecipitations.Add(hourData.GetProperty("precipitationSummary").GetProperty("pastHour").GetProperty("value").GetDouble());
            }

            return hourlyPrecipitations;
        }

        public async Task<double> GetPrecipitationAsync(double longitude, double latitude, int hoursPrior)
        {
            CheckValidHoursPrior(hoursPrior);
            List<double> hourlyPrecipitations = await GetHourlyPrecipitationAsync(longitude, latitude, hoursPrior).ConfigureAwait(false);
            return hourlyPrecipitations.Sum();
        }

        public async Task<double> GetPrecipitationAsync(double longitude, double latitude)
        {
            return await GetPrecipitationAsync(longitude, latitude, 0).ConfigureAwait(false);
        }

        public async Task<double> GetTemperatureAsync(double longitude, double latitude)
        {
            List<JsonElement> response = await CallAzureWeatherAPI(longitude, latitude).ConfigureAwait(false);
            return response.First().GetProperty("temperature").GetProperty("value").GetDouble();
        }

        private void CheckValidHoursPrior(int hoursPrior)
        {
            if (hoursPrior < 0)
            {
                throw new ArgumentException("The hoursPrior field cannot be negative");
            }
            else if (hoursPrior > 24)
            {
                if (hoursPrior < 0)
                {
                    throw new ArgumentException("The hoursPrior field cannot be greater than 24");
                }
            }
        }
    }
}