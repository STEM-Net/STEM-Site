using System;
using System.Collections.Generic;
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
        public IActionResult GetMoisture(string deviceId)
        {
            double moistureValue = 0;

            Sensor sensor = new Sensor();
            sensor.DeviceId = deviceId;
            sensor.Moisture = moistureValue;

            return PartialView("_PreviewPopup", sensor);
        }

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
