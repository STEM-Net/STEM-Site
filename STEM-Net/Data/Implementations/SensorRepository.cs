using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STEM_Net.Data
{
    class SensorRepository : ISensorRepository
    {
        private readonly string _connectionString;

        public SensorRepository(string connectionString)
        {
            this._connectionString = connectionString;
        }


    }
}
