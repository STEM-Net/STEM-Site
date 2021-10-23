using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Devices;
using STEM_Net.Models;

namespace STEM_Net.Controllers
{
    public class HomeController : Controller
    {
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

        [HttpPost]
        public IActionResult GetHistoricalData(string deviceId)
        {
            return PartialView("_HistoricalDataModal");
        }

        [HttpPost]
        public IActionResult GetMoisture(string deviceId)
        {
            Sensor sensor = new Sensor();
            //using (SqlConnection connection = new SqlConnection("Data Source=iotdataserverjoshie.database.windows.net;Initial Catalog=iotdata;User ID=joshie;Password=Pranav_12;Connect Timeout=60;Encrypt=True;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
            //{
            //    using (SqlCommand cmd = new SqlCommand("SELECT min(measureddatetime), avg(moisture) as moisture FROM SensorMoisture where deviceid=" + deviceId, connection))
            //    {
            //        try
            //        {
            //            connection.Open();
            //            SqlDataReader rdr = cmd.ExecuteReader();
            //            while (rdr.Read())
            //            {
            //                sensor.DeviceId = deviceId;
            //                sensor.Moisture = Convert.ToInt32(rdr["moisture"]);
            //                sensor.Name = "Tree Moisture Sensor";
            //            }
            //        }
            //        catch (Exception ex)
            //        {
            //            makeMockSensor(sensor, deviceId);
            //        }
            //    }
            //}
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

        //[HttpPost]
        //public IActionResult GetDetails(string deviceId)
        //{
        //    Dictionary<String, String> details = new Dictionary<String, String>();



        //    return deviceId;
        //}

        [HttpPost]
        async public Task<IActionResult> ListDevices()
        {
            var jsons = new List<string>();

            RegistryManager registryManager = RegistryManager.CreateFromConnectionString("HostName=PythonSimHub.azure-devices.net;SharedAccessKeyName=serviceAndRegistryReadWrite;SharedAccessKey=Ze+XP6e/2brJctHz0QuwjBSn3ywhx5FuazTPNzHiFWM=");

            var query = registryManager.CreateQuery("SELECT * FROM devices");
            while (query.HasMoreResults)
            {
                var page = await query.GetNextAsJsonAsync();
                foreach (var json in page)
                {
                    jsons.Add(json);
                }
            }

            return Ok(jsons);
        }
    }
}
