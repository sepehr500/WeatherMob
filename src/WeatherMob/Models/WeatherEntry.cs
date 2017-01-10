using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherMob.Models
{
    public enum PrecipType
    {
        Rain,
        Snow
    }

    public enum PrecipAmount
    {
        AtOrAboveAverage,
        BelowAverage

    }
    public class WeatherEntry
    {
        public int Id { get; set; }
        public int Hi { get; set; }
        public int Low { get; set; }
        public int CityId { get; set; } 
        public PrecipType? PrecipType { get; set; }
        public PrecipAmount? PrecipAmount{ get; set; }
        public bool Precip { get; set; }
        public DateTime PredictionDay { get; set; }
        public DateTime TargetDay { get; set; }
        public string UserId { get; set; }
        public virtual ApplicationUser ApplicationUser {get; set;}
        public virtual City City { get; set; }
    }
}
