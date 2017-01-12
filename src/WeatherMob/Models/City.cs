using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherMob.Models
{
    public class City
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string State { get; set; }
        public int TimeZone { get; set; }
        public string Airport { get; set; }
        public float Lat { get; set; }
        public float Long { get; set; }
        public float AvgAccum { get; set; }
        public List<WeatherEntry> WeatherEntries { get; set; }
        public List<AggregateWeatherPrediction> AggregateWeatherPredictions { get; set; }
        public List<ActualWeatherEntry> ActualWeatherEntries { get; set; }

    }
}
