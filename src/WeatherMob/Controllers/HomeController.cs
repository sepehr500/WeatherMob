using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WeatherMob.Classes;
using WeatherMob.Data;
using WeatherMob.Models;

namespace WeatherMob.Controllers
{
    public class HomeController : Controller
    {
        //private readonly UserManager<ApplicationUser> _userManager;
        public IActionResult Index()
        {
            var n = new DarkSky();
            return View();
        }

        [HttpPost]
        public IActionResult PostWeatherEntry(WeatherEntry entry)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                
                ClaimsPrincipal currentUser = User;
                if (currentUser.Identity.Name != null)
                {
                var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
                entry.UserId = currentUserID;
                    
                }
                if (ModelState.IsValid)
                {
                    db.WeatherEntries.Add(entry);
                    db.SaveChanges();
                    return StatusCode(200);
                }
                return Content("Failed To Create");
            }


        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
