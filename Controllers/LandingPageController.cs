using MarketPlace.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace MarketPlace.Controllers
{
    public class LandingPageController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }



    }
}
