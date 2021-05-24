using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STEM_Net.Models
{
    public class Sensor
    {
        public string DeviceId { get; set; }
        public double Longitude { get; set; }
        public double Latitutde { get; set; }
        public double Moisture { get; set; }

        public string Name { get; set; }
        public string ScientificName { get; set; }
        public string Genus { get; set; }
        public string Species { get; set; }
        public int TreeAge { get; set; }
        public double Diameter { get; set; }
        public double Height { get; set; }
        public double TrunkHeight { get; set; }
        public double TrunkDiameter { get; set; }
        public string CanopyShape { get; set; }
        public bool Stems { get; set; }
        public DateTime InstallDate { get; set; }
        public string MaintenanceNeed { get; set; }
        public double SpaceWidth { get; set; }
        public double SpaceLength { get; set; }
        public bool VacantSite { get; set; }
        public bool Active { get; set; }
        public DateTime LastUpdate { get; set; }
        public string Notes { get; set; }
    }
}
