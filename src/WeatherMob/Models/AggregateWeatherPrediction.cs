using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherMob.Models
{
    public class AggregateWeatherPrediction
    {
        public int Id { get; set; }
        public DateTime Day { get; set; }
        public double AvgHi { get; set; }
        public double AvgLow { get; set; }
        public double MedianHi { get; set; }
        public double MedianLow { get; set; }

        public double TotalYesPrecip { get; set; }
        public double TotalNoPrecip { get; set; }

        public int TotalRain { get; set; }
        public int TotalSnow { get; set; }

        public int AmtSmall { get; set; }
        public int AmtMed { get; set; }
        public int AmtLarge { get; set; }

    }
}
