using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using STEM_Net.Services;
using STEM_Net.Services.Implementations;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace TestSTEM_Net
{
    [TestClass]
    public class AzureWeatherAPITest
    {
        private static IWeatherService _azureWeather;
        private static HttpClient _client;

        [ClassInitialize()]
        public static void ClassInit(TestContext context)
        {
            var builder = new ConfigurationBuilder()
                .AddUserSecrets<AzureWeatherAPITest>();

            _client = new HttpClient();
            _azureWeather = new WeatherAzureService(_client, builder.Build());
        }

        [TestMethod]
        public async Task TestGetPrecipitation()
        {
            double precip = await _azureWeather.GetPrecipitationAsync(37.33811209295251, -121.88538465277796, 0).ConfigureAwait(false);
            Assert.IsTrue(precip >= 0);
            //System.Diagnostics.Debug.WriteLine(precip);
        }

        [TestMethod]
        public async Task TestGetHourlyPrecipitation()
        {
            List<double> precips = await _azureWeather.GetHourlyPrecipitationAsync(37.33811209295251, -121.88538465277796, 24).ConfigureAwait(false);
            Assert.IsTrue(precips.Count == 24);
            Assert.IsTrue(precips[0] >= 0);
        }

        [TestMethod]
        public async Task TestGetTemperature()
        {
            double temp = await _azureWeather.GetTemperatureAsync(37.33811209295251, -121.88538465277796).ConfigureAwait(false);
            Assert.IsTrue(temp > 15 && temp < 110);
            System.Diagnostics.Debug.WriteLine(temp);
        }

        [ClassCleanup()]
        public static void ClassCleanup()
        {
            _client.Dispose();
        }
    }
}
