using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STEM_Net.Models
{
    public class Filter
    {
        public double minMoisture { get; set; }
        public double maxMoisture { get; set; }

        public int minAge { get; set; }
        public int maxAge { get; set; }

        public double minDiameter { get; set; }
        public double maxDiameter { get; set; }

        public string species { get; set; }
        public string owneship { get; set; }
    }
}
