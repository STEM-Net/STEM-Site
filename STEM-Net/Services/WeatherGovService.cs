using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STEM_Net.Services
{
    public class WeatherGovService : IWeatherService
    {
        public float GetPrecipitation(float longitude, float latitude, int hoursPrior)
        {
            throw new NotImplementedException();
        }

        public float GetTemperature(float longitude, float latitude)
        {
            throw new NotImplementedException();
        }
    }
}
