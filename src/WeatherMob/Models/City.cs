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
        public int TimeZone { get; set; }

        public List<WeatherEntry> WeatherEntries { get; set; }
    }
}
