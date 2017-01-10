using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeatherMob.Data;
using WeatherMob.Models;
using ForecastIO;
using Microsoft.EntityFrameworkCore.Query.ExpressionTranslators.Internal;

namespace WeatherMob.Classes
{
    public interface IActualWeatherApi
    {
        ActualWeatherEntry GetActualWeatherEntryApi(float Lat, float Long, DateTime day, float AvgAccum);
    }

    public  class  DarkSky: IActualWeatherApi
    {
        public ActualWeatherEntry GetActualWeatherEntryApi(float Lat , float Long , DateTime day , float AvgAccum)
        {
            var excludeBlocks = new Exclude[]
            {
                Exclude.alerts,
                Exclude.currently
            };
                
            var request = new ForecastIORequest("4ad8e97b2cd0133665a1e30b43fa7973", Lat, Long, day, Unit.us);
            var response = request.Get().daily;
            var returnObj = new ActualWeatherEntry();
            var dayWeather = response.data.First();
            returnObj.ActualHi = dayWeather.temperatureMax;
            returnObj.ActualLow = dayWeather.temperatureMin;
            returnObj.ActualPrecip = dayWeather.precipType != null;
            if (dayWeather.precipType != null)
            {
                returnObj.ActualPrecipType = dayWeather.precipType == "rain" || dayWeather.precipType == "sleet"
                    ? PrecipType.Rain
                    : PrecipType.Snow;
            }
            if (returnObj.ActualPrecipType != null && returnObj.ActualPrecipType == PrecipType.Snow)
            {

                returnObj.ActualPrecipAmount = dayWeather.precipAccumulation > AvgAccum
                    ? PrecipAmount.AtOrAboveAverage
                    : PrecipAmount.BelowAverage;
            }
            return returnObj;
        }
    }

    public class RecordActualWeather
    {
        public static void GetActualWeather()
        {
            var InjectedApi = new DarkSky();
            using (var db = new ApplicationDbContext())
            {
                foreach (var city in db.Cities.ToList())
                {
                    var startDay = DateTime.Now.AddHours(city.TimeZone);
                    //If there no no entries for that date
                    if (db.ActualWeatherEntries.Any(z => z.Day.DayMonthYearCheck(startDay)) ==false)
                    {
                        var currentDayWeather = InjectedApi.GetActualWeatherEntryApi(city.Lat, city.Long,startDay,city.AvgAccum);
                        currentDayWeather.City = city;
                        currentDayWeather.CityId = city.Id;
                        currentDayWeather.Day = startDay;
                        db.ActualWeatherEntries.Add(currentDayWeather);

                    }
                }
                db.SaveChanges();
            }
        }
    }
}
