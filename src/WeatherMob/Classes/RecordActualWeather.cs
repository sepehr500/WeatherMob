using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeatherMob.Data;
using WeatherMob.Models;
using ForecastIO;
using Microsoft.EntityFrameworkCore.Query.ExpressionTranslators.Internal;
using HtmlAgilityPack;

namespace WeatherMob.Classes
{
    public interface IActualWeatherApi
    {
        ActualWeatherEntry GetActualWeatherEntryApi(float Lat, float Long, DateTime day);
    }

    class WUGet
    {
        public ActualWeatherEntry GetActualWeatherEntryApi(string airport, string city , string state, DateTime day)
        {

            HtmlWeb web = new HtmlWeb();
            var thing =  string.Format(
                "https://www.wunderground.com/history/airport/{0}/{1}/{2}/{3}/DailyHistory.html?req_city={4}&req_statename={5}",
                airport, day.Year, day.Month, day.Day, city, state);
            HtmlDocument document = web.Load(thing);
            //HtmlDocument document =  web.Load("https://www.wunderground.com/history/airport/KDCA/2017/1/3/DailyHistory.html");
            var mainTable =  document.DocumentNode.SelectNodes("//table[@id='historyTable']").First();

            var minTemp = mainTable.ChildNodes[3].ChildNodes[7].ChildNodes[3].ChildNodes[1].ChildNodes[0].InnerHtml;
            var maxTemp = mainTable.ChildNodes[3].ChildNodes[5].ChildNodes[3].ChildNodes[1].ChildNodes[0].InnerHtml;
            var Evnt = "";
            var snow = "";
            try
            {
            snow = mainTable.ChildNodes[3].ChildNodes[43].ChildNodes[3].ChildNodes[1].ChildNodes[0].InnerHtml;
            Evnt = mainTable.ChildNodes[3].ChildNodes[65].ChildNodes[3].ChildNodes[0].InnerHtml.Trim();

            }
            catch (Exception)
            {
            snow = "none";
                //Total amount of precip if there is no now column
            var amt = mainTable.ChildNodes[3].ChildNodes[37].ChildNodes[3].ChildNodes[1].ChildNodes[0].InnerHtml.Trim();
                Evnt = mainTable.ChildNodes[3].ChildNodes[57].ChildNodes[3].ChildNodes[0].InnerHtml.Trim();
                
            }
            var returnObj = new ActualWeatherEntry();
            returnObj.ActualHi = Convert.ToSingle(maxTemp);
            returnObj.ActualLow = Convert.ToSingle(minTemp);
            returnObj.ActualPrecip = Evnt == "Snow" || Evnt == "Rain";
            if (Evnt == "Snow")
            {
                returnObj.ActualPrecipAmount = Convert.ToSingle(snow);
            }

            return returnObj;
        }
    }

    public  class  DarkSky: IActualWeatherApi
    {
        public ActualWeatherEntry GetActualWeatherEntryApi(float Lat , float Long , DateTime day )
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
                returnObj.ActualPrecipType = dayWeather.precipType;
            }
            if (returnObj.ActualPrecipType != null && returnObj.ActualPrecipType == "snow")
            {

                returnObj.ActualPrecipAmount = dayWeather.precipAccumulation;
            }
            return returnObj;
        }
    }

    public class RecordActualWeather
    {
        public static void GetActualWeather()
        {
            var InjectedApi = new WUGet();
            using (var db = new ApplicationDbContext())
            {
                foreach (var city in db.Cities.ToList())
                {
                    var startDay = DateTime.Now.AddHours(city.TimeZone).AddDays(-1);
                    //If there no no entries for that date
                    if (db.ActualWeatherEntries.Any() ==false)
                    {
                        var currentDayWeather = InjectedApi.GetActualWeatherEntryApi(city.Airport , city.Name , city.State , startDay);
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
