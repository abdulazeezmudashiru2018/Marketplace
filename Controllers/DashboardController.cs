

using Microsoft.AspNetCore.Mvc;

namespace MarketPlace.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}