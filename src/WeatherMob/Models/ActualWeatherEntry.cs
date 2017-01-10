using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherMob.Models
{
    public class ActualWeatherEntry
    {
        public int Id { get; set; }
        public DateTime Day { get; set; }
        public float ActualHi { get; set; }
        public float ActualLow { get; set; }
        
        public PrecipType? ActualPrecipType { get; set; }
        public PrecipAmount? ActualPrecipAmount { get; set; }
        
        public bool ActualPrecip { get; set; }
        public int CityId { get; set; }

        public virtual City City { get; set; }

    }
}
