using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Devices;
using Microsoft.Extensions.Configuration;
using STEM_Net.Data;
using STEM_Net.Models;
using STEM_Net.Services;

namespace STEM_Net.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWeatherService _weatherService;
        private readonly ISensorRepository _sensorRepository;

        public HomeController(ISensorRepository sensorRepository, IWeatherService weatherService)
        {
            _sensorRepository = sensorRepository;
            _weatherService = weatherService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        
        public IActionResult TestView()
        {
            return View();
        }

        /// <summary>
        /// Gets a list of all the trees in the dataset that fall within the specifications of the filter.
        /// </summary>
        /// <param name="filter">The filter that specifies which values or range of values should be returned.</param>
        /// <returns>An HTTP response listing out all the trees that meet the criteria set in the filter.</returns>
        [HttpPost]
        public IActionResult GetTrees(Filter filter) {

            //using (DbConnection connection = new MySqlConnection("Server=\"stemnetsensorpushdb.mysql.database.azure.com\";Port=3306;UserID=\"stemnetwork\";Password=\"{}\";Database=\"{stemnettestdb}\";"))
            //{
            //    connection.Open();
            //    using (DbCommand cmd = connection.CreateCommand())
            //    {
            //        cmd.CommandText = "SELECT min(measureddatetime), avg(moisture) as moisture FROM SensorMoisture where deviceid=" + deviceId;

            //        using (var reader = cmd.ExecuteReader())
            //        {
            //            while (reader.Read())
            //            {
            //                //sensor.DeviceId = deviceId;
            //                //sensor.Moisture = Convert.ToInt32(reader["moisture"]);
            //                //sensor.Name = "Tree Moisture Sensor";
            //            }
            //        }
            //    }
            //}

            return Ok();
        }

        [HttpPost]
        public IActionResult GetTabularView(Filter filter) {

            return PartialView("_DataTable", new List<Sensor>());
        }

        /// <summary>
        /// Returns the historical data modal for the device with the given ID.
        /// </summary>
        /// <param name="deviceId">The device ID of the sensor whose historical data we'd like to retrieve.</param>
        /// <returns>The partial view displaying the historical data modal.</returns>
        [HttpPost]
        public IActionResult GetHistoricalData(string deviceId)
        {
            Sensor sensor = new Sensor();
            makeMockSensor(sensor, deviceId);
            return PartialView("_HistoricalDataModal", sensor);
        }

        /// <summary>
        /// Gets the latest moisture reading of the device with the given ID.
        /// </summary>
        /// <param name="deviceId">The device ID of the sensor whose moisture reading we'd like to retrieve.</param>
        /// <returns>The partial view displaying the pop-up containing the moisture data of the given device.</returns>
        [HttpPost]
        public IActionResult GetMoisture(string deviceId)
        {
            Sensor sensor = new Sensor();
            using (DbConnection connection = new SqlConnection("Server=\"stemnetcentraldb.database.windows.net\";Port=3306;UserID=\"stemnetwork\";Password=\"{}\";Database=\"{stemnettestdb}\";"))
            {
                connection.Open();
                using (DbCommand cmd = connection.CreateCommand())
                {
                    try
                    {
                        cmd.CommandText = "SELECT min(measureddatetime), avg(moisture) as moisture FROM SensorMoisture where deviceid=" + deviceId;

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                sensor.DeviceId = deviceId;
                                sensor.Moisture = Convert.ToInt32(reader["moisture"]);
                                sensor.Name = "Tree Moisture Sensor";
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        makeMockSensor(sensor, deviceId);
                    }
                }
            }
            makeMockSensor(sensor, deviceId);
            return PartialView("_PreviewPopup", sensor);
        }

        // TODO: Delete this
        private void makeMockSensor(Sensor sensor, string deviceId)
        {
            sensor.DeviceId = deviceId;
            Random random = new Random(deviceId.GetHashCode());
            sensor.Moisture = random.Next(60);
            sensor.Name = "Tree Moisture Sensor";
        }

        [HttpGet]
        public async Task<IActionResult> GetCurrentTemperature(double longitude, double latitude)
        {
            return Ok(await _weatherService.GetTemperatureAsync(longitude, latitude).ConfigureAwait(false));
        }

        [HttpGet]
        public async Task<IActionResult> GetCurrentPrecipitation(double longitude, double latitude)
        {
            return Ok(await _weatherService.GetPrecipitationAsync(longitude, latitude).ConfigureAwait(false));
        }

        [HttpGet]
        public async Task<IActionResult> GetPrecipitationHourly(double longitude, double latitude, int hours)
        {
            return Ok(await _weatherService.GetHourlyPrecipitationAsync(longitude, latitude, hours).ConfigureAwait(false));
        }

        [HttpGet]
        public async Task<IActionResult> Get24HourPrecipitation(double longitude, double latitude, int hours)
        {
            List<double> hourlyPrecip = await _weatherService.GetHourlyPrecipitationAsync(longitude, latitude, hours).ConfigureAwait(false);
            return Ok(hourlyPrecip.Sum());
        }

        //[HttpPost]
        //public IActionResult GetDetails(string deviceId)
        //{
        //    Dictionary<String, String> details = new Dictionary<String, String>();



        //    return deviceId;
        //}  [HttpPost]
        async public Task<IActionResult> ListDevices()
        {
            var jsons = new List<string>();

            RegistryManager registryManager = RegistryManager.CreateFromConnectionString("HostName=PythonSimHub.azure-devices.net;SharedAccessKeyName=serviceAndRegistryReadWrite;SharedAccessKey=Ze+XP6e/2brJctHz0QuwjBSn3ywhx5FuazTPNzHiFWM=");

            var query = registryManager.CreateQuery("SELECT * FROM devices");
            while (query.HasMoreResults)
            {
                var page = await query.GetNextAsJsonAsync().ConfigureAwait(false);
                foreach (var json in page)
                {
                    jsons.Add(json);
                }
            }

            return Ok(jsons);
        }
    }
}
