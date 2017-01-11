using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeatherMob.Data;
using WeatherMob.Models;
using LinqStatistics;
namespace WeatherMob.Classes
{
    public static class DayUpdate
    {
        private const int DaysIntoFuture = 15;

        public static bool DayMonthYearCheck(this DateTime x, DateTime y)
        {

            return (x.Day == y.Day && x.Month == y.Month && x.Year == y.Year);
        }

        public static void Update()
        {
            using (var db = new ApplicationDbContext())
            {
                foreach (var city in db.Cities.ToList())
                {
                    //Add day to local time zone adjusted to get next day to start on.
                    var startDay = DateTime.Now.AddHours(city.TimeZone);
                    for (int i = 0; i < DaysIntoFuture; i++)
                    {
                        startDay = startDay.AddDays(1);
                        var entries = db.WeatherEntries.Where(k => k.CityId == city.Id && DayMonthYearCheck(k.TargetDay , startDay));
                        if (entries.Any())
                        {
                                var updateEntry = db.AggregateWeatherPredictions.FirstOrDefault(y => DayMonthYearCheck(y.Day, startDay));
                            if (updateEntry != null)
                            {

                                updateEntry.Day = startDay;
                                    updateEntry.AvgHi = entries.Average(n => n.Hi);
                                    updateEntry.AvgLow = entries.Average(y => y.Low);
                                    updateEntry.MedianHi = entries.Median(n => n.Hi);
                                    updateEntry.MedianLow = entries.Median(n => n.Low);
                                    updateEntry.TotalNoPrecip = entries.Count(n => n.Precip == false);
                                    updateEntry.TotalYesPrecip = entries.Count(n => n.Precip == true);
                                    updateEntry.TotalRain = entries.Count(n => n.PrecipType == PrecipType.Rain);
                                    updateEntry.TotalSnow = entries.Count(n => n.PrecipType == PrecipType.Snow);
                                    updateEntry.PrecipAmount = entries.Average(n => n.PrecipAmount);


                                db.AggregateWeatherPredictions.Update(updateEntry);
                            }
                            else
                            {
                                
                            db.AggregateWeatherPredictions.Add(new AggregateWeatherPrediction()
                            {
                            CityId = city.Id,
                            Day = startDay,
                            AvgHi = entries.Average(n => n.Hi),
                            AvgLow = entries.Average(y => y.Low),
                            MedianHi = entries.Median(n => n.Hi),
                            MedianLow = entries.Median(n => n.Low),
                            TotalNoPrecip = entries.Count(n => n.Precip == false),
                            TotalYesPrecip = entries.Count(n => n.Precip == true),
                            TotalRain = entries.Count(n => n.PrecipType == PrecipType.Rain ),
                            TotalSnow = entries.Count(n => n.PrecipType == PrecipType.Snow ),
                            PrecipAmount = entries.Average(n => n.PrecipAmount)


                            });
                            }
                        }

                    }


                }
               db.SaveChanges();
            }
        }
    }
}
