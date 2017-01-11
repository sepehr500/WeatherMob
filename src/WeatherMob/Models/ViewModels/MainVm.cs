using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherMob.Models.ViewModels
{
    public class MainVm
    {
        public List<City> Cities { get; set; }
        public List<ActualWeatherEntry> ActualWeatherEntries { get; set; }
        public List<AggregateWeatherPrediction> AggregateWeatherPredictions { get; set; }
        public List<WeatherEntry> WeatherEntries { get; set; }
    }
}
